module Viewer.Utils

open System
open Viewer.Types

let extractFilters qs =
  qs
  |> Seq.map (fun (k,v) ->
                match v with
                  | Some s -> {Key = k; Val = s}
                  | None ->   {Key = k; Val = ""})
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
    |> Seq.filter (fun y -> y.Val <> x)
    |> Seq.map (fun y -> sprintf "%s=%s" y.Key (Uri.EscapeDataString(y.Val)))
    |> concatToStringWithDelimiter "&"

  try
    filters
    |> Seq.map (fun x -> {Label = x.Val.Split('#').[1]
                          RemovalQueryString = createRemovalQS x.Val})
    |> Seq.toList
  with
    | _ -> []
