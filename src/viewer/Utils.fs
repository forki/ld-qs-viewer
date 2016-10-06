module Viewer.Utils

open System
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration

let extractFilters qs =
  qs
  |> Seq.map (fun (k,v) ->
                match v with
                  | Some s -> {Vocab = k; TermUri = s}
                  | None ->   {Vocab = k; TermUri = ""}
              )
  |> Seq.toList

let aggregateQueryStringValues filters =
  let getTerms (k, vals) = 
    let aggregatedVals = vals
                         |> Seq.map (fun (v:Filter) -> v.TermUri)
                         |> Seq.toList
    (k, aggregatedVals)

  filters
  |> Seq.groupBy (fun k -> k.Vocab)
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


let createFilterTags (filters:Filter list) vocabs =

  let createRemovalQS x =
    printf "TermUri %A" x
    filters
    |> Seq.filter (fun y -> y.TermUri <> x)
    |> Seq.map (fun y -> sprintf "%s=%s" y.Vocab (Uri.EscapeDataString(y.TermUri)))
    |> concatToStringWithDelimiter "&"

  filters 
  |> Seq.map (fun x->{ Label = try Seq.head (findTheLabel vocabs (x.TermUri.Split('/').[1]) ) with _ -> ""
                       RemovalQueryString = createRemovalQS x.TermUri})
  |> Seq.filter (fun x -> x.Label <> "")
  |> Seq.toList


let shouldExpandVocab vocabProperty (filters:Filter list) =
  filters |> List.exists (fun x -> (System.Uri.UnescapeDataString x.Vocab) = vocabProperty)
