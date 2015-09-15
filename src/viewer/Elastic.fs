module Viewer.Elastic

open Viewer.Types
open Elasticsearch.Net
open Elasticsearch.Net.Connection
open System
open FSharp.Data

let RunElasticQuery (query: string) =
  Http.RequestString(query)



let GetSearchResults runSearchWith query =
  let queryResponse = runSearchWith query
  try
    let json = FSharp.Data.JsonProvider<"elasticResponseSchema.json">.Parse(queryResponse)
    let hits = json.Hits.Hits
    hits
      |> Seq.map(fun x -> {Uri = x.Source.Id})
      |> Seq.toList
  with
    | _ -> []
