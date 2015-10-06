module Viewer.Utils

let extractFilters qs =
  qs
  |> Seq.map (fun (_,v) ->
                match v with
                  | Some s -> s
                  | None -> "")
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
