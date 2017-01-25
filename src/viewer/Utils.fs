module Viewer.Utils

open Serilog
open NICE.Logging
open System
open System.Text
open System.Text.RegularExpressions
open Microsoft.FSharp.Core.Printf
open System.Reflection
open Viewer.Types
open Viewer.YamlParser
open Suave


let formatDisplayMessage (e:Exception) =
    let sb = StringBuilder()
    let delimeter = String.replicate 50 "*"
    let nl = Environment.NewLine
    let rec printException (e:Exception) count =
        if (e :? TargetException && e.InnerException <> null)
        then printException (e.InnerException) count
        else
            if (count = 1) then bprintf sb "%s%s%s" e.Message nl delimeter
            else bprintf sb "%s%s%d)%s%s%s" nl nl count e.Message nl delimeter
            bprintf sb "%sType: %s" nl (e.GetType().FullName)
            // Loop through the public properties of the exception object
            // and record their values.
            e.GetType().GetProperties()
            |> Array.iter (fun p ->
                // Do not log information for the InnerException or StackTrace.
                // This information is captured later in the process.
                if (p.Name <> "InnerException" && p.Name <> "StackTrace" &&
                    p.Name <> "Message" && p.Name <> "Data") then
                    try
                        let value = p.GetValue(e, null)
                        if (value <> null)
                        then bprintf sb "%s%s: %s" nl p.Name (value.ToString())
                    with
                    | e2 -> bprintf sb "%s%s: %s" nl p.Name e2.Message
            )
            if (e.StackTrace <> null) then
                bprintf sb "%s%sStackTrace%s%s%s" nl nl nl delimeter nl
                bprintf sb "%s%s" nl e.StackTrace
            if (e.InnerException <> null)
            then printException e.InnerException (count+1)
    printException e 1
    sb.ToString()

let extractFilters qs =
  qs
  |> Seq.map (fun (k,v) ->
                match v with
                  | Some s -> {Vocab = k; TermUri = s}
                  | None ->   {Vocab = k; TermUri = ""}
              )
  |> Seq.toList

let aggregateFiltersByVocab (filters:Filter list) =
  let getTerms (v, terms) = 
    let aggregatedVals = terms
                         |> Seq.map (fun (f:Filter) -> f.TermUri)
                         |> Seq.toList
    {Vocab=v
     TermUris=aggregatedVals}

  filters
  |> Seq.groupBy (fun f -> f.Vocab)
  |> Seq.map getTerms
  |> Seq.toList

let insertItemsMultipleInto query item1 item2 =
  sprintf (Printf.StringFormat<string->string->string->string->string>(query)) item1 item2 item1 item2

let insertItemsInto query item1 item2 =
  sprintf (Printf.StringFormat<string->string->string>(query)) item1 item2

let insertItemInto query item =
  sprintf (Printf.StringFormat<string->string>(query)) item

let concatToStringWithDelimiter delimiter items = 
  items
  |> Seq.fold (fun acc item ->
               match acc with
                 | "" -> item
                 | _ -> acc + delimiter + item) ""
    
let getGuidFromShortUri (shortUri:string) =
  try shortUri.Split('/').[1] with _ -> ""

let flattenVocab (vocabs:Vocabulary List) =
  let getTerms (vocab:Vocabulary) =
    match vocab.Root with
    | Term t -> [Term t]
    | _ -> []

  let rec flattenTerms (terms:Term List) =
    terms
    |> List.map (fun x -> match x with
                          | Term t -> match t.Children with
                                      | [] -> [Term t]
                                      | _ -> flattenTerms ([Term {t with Children = []}] @ t.Children)
                          | _ -> [] )
    |> List.concat
      
  vocabs |> List.map getTerms
         |> List.concat
         |> flattenTerms
         |> List.map (fun x -> match x with
                               | Term t -> [{ Label = t.Label; ShortUri = t.ShortenedUri; Guid = getGuidFromShortUri t.ShortenedUri }]
                               | _ -> [])
         |> List.concat

let private getVocabLookup flatvocab shortUri =
  flatvocab
  |> List.tryFind (fun x -> x.ShortUri = shortUri)
  |> (fun x -> match x with
                          | None -> { Label = ""; ShortUri = shortUri; Guid = "" }
                          | _ -> x.Value)

let getGuidFromFilter (filter:Filter) =
  try filter.TermUri.Split('/').[1] with _ -> ""

let getLabelFromGuid (vocabs:vocabLookup List) (filter:Filter) = 
  //try Seq.head (findTheLabel vocabs (getGuidFromFilter filter)) with _ -> ""
  vocabs
  |> List.tryFind( fun x -> x.ShortUri = filter.TermUri)
  |> fun x -> match x with
              | None -> "NO MATCH FOUND"
              | Some v -> v.Label

let createFilterTags (filters:Filter list) vocabs =
  let flatVocabLookup = getVocabLookup (flattenVocab vocabs)
  let createRemovalQS x =
    filters
    |> Seq.filter (fun y -> y.TermUri <> x)
    |> Seq.map (fun y -> sprintf "%s=%s" y.Vocab (Uri.EscapeDataString(y.TermUri)))
    |> concatToStringWithDelimiter "&"

  filters 
  |> Seq.map (fun x -> flatVocabLookup x.TermUri |> fun y -> { Label = y.Label; RemovalQueryString = createRemovalQS x.TermUri})
  |> Seq.filter (fun x -> x.Label <> "")
  |> Seq.toList

let findTheGuid vocabs filterUri =
  let rec getTerm f = function
    | [] -> []
    | x::xs -> match x with
                | Term x -> if f x then 
                              [x]; 
                            else 
                              match xs with
                              | [] -> getTerm f x.Children
                              | _ -> if x.Children <> [] then getTerm f x.Children else getTerm f xs
                | Empty -> []
  vocabs
  |> List.map (fun v -> getTerm (fun t->t.Label=filterUri) [v.Root]) 
  |> List.concat
  |> List.map (fun t -> try t.ShortenedUri.Split('/').[1] with _ -> "") 

let getGuids (labels:string list) vocabs =
  let flatVocabs = flattenVocab vocabs 

  let getGuid label =
     flatVocabs |> List.tryFind (fun x -> x.Label = label)
                |> (fun x -> match x with
                             | None -> ""
                             | _ -> x.Value.Guid)

  labels 
  |> Seq.map getGuid
  |> Seq.filter (fun x -> x <> "")
  |> Seq.toList

let shouldExpandVocab vocabProperty (filters:Filter list) =
  filters |> List.exists (fun x -> (System.Uri.UnescapeDataString x.Vocab) = vocabProperty)

let appendRootUrl queryString =
  "/annotationtool/toyaml?" + queryString

let getItems searchByProperty getProperty searchTerms vocabs =
  let searchFn filters searchByProperty x =
    Seq.exists (fun a ->a = searchByProperty x) filters

  let search currentTerm =
   searchFn searchTerms searchByProperty currentTerm
  
  let rec recurseTree start children =
    List.fold (fun acc term ->  match term with
                                | Term term -> if (search term) then
                                                  [getProperty term] @ acc @ recurseTree start term.Children
                                                else
                                                  acc @ recurseTree start term.Children
                                | Empty -> []) start children

  recurseTree [] vocabs

let private getKey (vocabs: Vocabulary list) (name:string) =

  let foundVocabs =
    vocabs
    |> Seq.filter (fun x->x.Label=name.Trim())
    |> Seq.toList

  match foundVocabs with
  | [] -> "NO MATCH FOUND"
  | _ -> foundVocabs |> List.head |> (fun v -> v.Property)
    
let private getValue vocabs field =

  let foundTerms = 
    vocabs
    |> Seq.map (fun v->v.Root)
    |> Seq.toList
    |> getItems (fun v->try v.ShortenedUri.Split('/').[1] with _ -> v.ShortenedUri) (fun t->t.ShortenedUri) [field]

  match foundTerms with
  | [] -> ""
  | _ -> Seq.head foundTerms

let private transformYamlToUrl getKeyFn getValueFn yaml =

  let buildUrl (name:string) fields =
    List.fold (fun acc field -> acc + getKeyFn name + "=" + getValueFn field + "&" ) "" fields

  yaml
  |> parseYaml 
  |> Seq.map (fun x->buildUrl x.Name x.Fields )
  |> Seq.concat
  |> Seq.toArray
  |> System.String
  |> fun x->x.[0..x.Length-2]

let getRedirectUrl (vocabs:Vocabulary list) yaml =
    yaml
    |> transformYamlToUrl (getKey vocabs) (getValue vocabs)
    |> appendRootUrl 

let getQueryStringFromYaml (vocabs: Vocabulary list) req =
  let getYamlFromPostedData formData = 
    snd formData

  List.head req.multiPartFields 
  |> getYamlFromPostedData  
  |> getRedirectUrl vocabs

