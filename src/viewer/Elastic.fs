module Viewer.Elastic

open Viewer.Types
open Elasticsearch.Net
open Elasticsearch.Net.Connection
open System

let RunElasticQuery (query: string) =
  let uri = new Uri("http://localhost:9200")
  let settings = new ConnectionConfiguration(uri) |> (fun x -> x.ExposeRawResponse())
  let client = new ElasticsearchClient(settings) 
  let index = "kb"
  let response = client.Search<string>(index, query)
  response.Response

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

