module Viewer.Utils

open System
open Viewer.Types

let extractFilters qs =
  qs
  |> Seq.map (fun (k,v) ->
                match v with
                  | Some s -> {Vocab = k; TermUri = s}
                  | None ->   {Vocab = k; TermUri = ""})
  |> Seq.toList

let aggregateQueryStringValues qsPairs =
  let createAggregatedValuesForKey (k, vals) = 
    let aggregatedVals = vals
                         |> Seq.map (fun (_,p) ->
                                     match p with
                                       | Some s -> s)
                         |> Seq.toList
    (k, aggregatedVals)

  qsPairs
  |> Seq.groupBy (fun (k,_) -> k)
  |> Seq.map createAggregatedValuesForKey
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


let createFilterTags filters =

  let createRemovalQS x =
    filters
    |> Seq.filter (fun y -> y.TermUri <> x)
    |> Seq.map (fun y -> sprintf "%s=%s" y.Vocab (Uri.EscapeDataString(y.TermUri)))
    |> concatToStringWithDelimiter "&"

  filters
  |> Seq.map (fun x -> {Label = try x.TermUri.Split('#').[1] with _ -> ""
                        RemovalQueryString = createRemovalQS x.TermUri})
  |> Seq.filter (fun x -> x.Label <> "")
  |> Seq.toList
