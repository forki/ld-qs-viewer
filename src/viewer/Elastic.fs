module Viewer.Elastic

open Viewer.Utils
open Viewer.Types
open Viewer.Queries
open Elasticsearch.Net
open Elasticsearch.Net.Connection
open System
open FSharp.Data

let BuildQuery qsPairs =
  let aggregatedKeyValues = aggregateQueryStringValues qsPairs 

  let shouldQuery = aggregatedKeyValues
                    |> Seq.map (fun (k, vals) -> vals
                                                 |> Seq.map (fun v -> insertItemsInto termQuery k v)
                                                 |> concatToStringWithDelimiter ",")
                    |> Seq.map (fun termQueriesStr -> insertItemInto shouldQuery termQueriesStr)
                    |> concatToStringWithDelimiter ","

  let fullQuery = insertItemInto mustQuery shouldQuery

  printf "Running query: %s" fullQuery
  fullQuery

let RunElasticQuery (query: string) =
  Http.RequestString("http://elastic:9200/kb/qualitystatement/_search?", body = TextRequest query)

let ParseResponse response = 

  let chopPath (url:string) =
    try
      //This should probably be done elsewhere!
      //converting from "http://ld.nice.org.uk/prov/entity#98ead3d:qualitystandards/qs7/st2/Statement.md" 
      //to = "/qualitystandards/qs7/st2/Statement.html" 
      let parts = url.Split (':')
      let path = parts.[2]
      let id = path.Split('.')
      sprintf "/%s.html" id.[0]
    with
      | ex -> url

  try
    let json = FSharp.Data.JsonProvider<"elasticResponseSchema.json">.Parse(response)
    let hits = json.Hits.Hits
    hits
      |> Seq.map(fun hit -> {Uri = chopPath hit.Source.Id;Abstract = hit.Source.DctermsAbstract})
      |> Seq.toList
  with
    | ex ->
      printf "%s" (ex.ToString())
      []

let GetSearchResults runSearch query =
  query |> runSearch |> ParseResponse
