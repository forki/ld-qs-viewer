module Viewer.Utils

open System
open Viewer.Types

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

let getLabelFromGuid vocabs (filter:Filter) = 
  try Seq.head (findTheLabel vocabs (getGuidFromFilter filter)) with _ -> ""

let createFilterTags (filters:Filter list) vocabs =

  let createRemovalQS x =
    filters
    |> Seq.filter (fun y -> y.TermUri <> x)
    |> Seq.map (fun y -> sprintf "%s=%s" y.Vocab (Uri.EscapeDataString(y.TermUri)))
    |> concatToStringWithDelimiter "&"

  filters 
  |> Seq.map (fun x->{ Label = try Seq.head (findTheLabel vocabs (x.TermUri.Split('/').[1]) ) with _ -> ""
                       RemovalQueryString = createRemovalQS x.TermUri})
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
                              | _ -> getTerm f xs
                | Empty -> []
  vocabs
  |> List.map (fun v -> getTerm (fun t->t.Label=filterUri) [v.Root]) 
  |> List.concat
  |> List.map (fun t -> try t.ShortenedUri.Split('/').[1] with _ -> "") 

let getGuids (labels:string list) vocabs =

  labels 
  |> Seq.map (fun x-> try Seq.head (findTheGuid vocabs (x) ) with _ -> "")
  |> Seq.filter (fun x -> x <> "")
  |> Seq.toList

let shouldExpandVocab vocabProperty (filters:Filter list) =
  filters |> List.exists (fun x -> (System.Uri.UnescapeDataString x.Vocab) = vocabProperty)

