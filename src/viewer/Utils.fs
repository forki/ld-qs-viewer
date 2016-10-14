module Viewer.Utils

open Serilog
open NICE.Logging
open System
open Viewer.Types
open Viewer.YamlParser

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

let stripAllButFragment (uri:string) =
    let from = uri.LastIndexOf("#") + 1
    let toEnd = uri.Length - from
    uri.Substring(from, toEnd)
    
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

//let findTheLabel vocabs (filterUris:string) =
//  let flatVocabLookup = getVocabLookup (flatternVocab (fun x -> x) vocabs)
//
//  [filterUris]
//  |> List.map flatVocabLookup
//  |> List.filter (fun x -> x.Label <> "")
//  |> List.map (fun x -> x.Label)

let findTheLabel vocabs filterUris =
  let rec getTerm fn = function
    | [] -> []
    | x::xs -> match x with
                | Term x -> if fn x then 
                              [x]; 
                            else 
                              match xs with
                              | [] -> getTerm fn x.Children
                              | _ -> if x.Children = [] then getTerm fn xs else getTerm fn x.Children
                | Empty -> []
  vocabs
  |> List.map (fun v -> getTerm (fun t->t.ShortenedUri.Contains(filterUris)) [v.Root]) 
  |> List.concat
  |> List.map (fun t -> t.Label) 
  |> List.filter (fun l->l <> "")

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

let transformYamlToUrl (yaml:string) =
  let y = parseYaml yaml
  let escapeString s =
    System.Uri.EscapeDataString s

  let mapFields  start fields = 
    Seq.fold (fun acc x -> x + acc) start fields

  y |> Seq.fold (fun _ x -> 
                      let key = escapeString ("qualitystandard:appliesTo" + x.Name)
                      let value = escapeString (x.Name.ToLower() + "/" + (mapFields "" x.Fields)) 
                      key + "=" + value) ""

