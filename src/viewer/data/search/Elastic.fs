module Viewer.Data.Search.Elastic

open Viewer.Utils
open Viewer.Types
open Viewer.Data.Search.Queries
open System
open FSharp.Data
open System.Text.RegularExpressions

let BuildQuery qsPairs =
  let aggregatedKeyValues = aggregateQueryStringValues qsPairs 

  let shouldQuery = aggregatedKeyValues
                    |> Seq.map (fun (k, vals) -> vals
                                                 |> Seq.map (fun v -> insertItemsInto termQuery (Uri.UnescapeDataString k) v)
                                                 |> concatToStringWithDelimiter ",")
                    |> Seq.map (fun termQueriesStr -> insertItemInto shouldQuery termQueriesStr)
                    |> concatToStringWithDelimiter ","

  let fullQuery = insertItemInto mustQuery shouldQuery

  fullQuery

let GetKBCount testing =
  let indexName =
    match testing with
    | true -> "kb_test"
    | false -> "kb"
  let url = sprintf "http://elastic:9200/%s/_count?" indexName
  try
    Http.RequestString(url)
  with
    ex ->
      printf "ELASTICSEARCH_ERROR - unable to get count check index%s\n"
      ""

let RunElasticQuery testing (query: string) =
  let indexName =
    match testing with
    | true -> "kb_test"
    | false -> "kb"

  let url = sprintf "http://elastic.elasticsearch:9200/%s/qualitystatement/_search?" indexName
  try
    Http.RequestString(url,
                       body = TextRequest query,
                       headers = [ "Content-Type", "application/json;charset=utf-8" ])
  with
    | ex ->
      printf "ELASTICSEARCH_ERROR - unable to execute query%s\n"
      ""

let ParseCountResponse resp =
  try
    let json = JsonProvider<"data/search/elasticCountSchema.json">.Parse(resp)
    json.Count
  with
    | ex ->
      printf "ELASTICSEARCH_ERROR - unable to parse response for count\n"
      0

let ParseResponse response =
  let chopPath (url:string) =
    try
      ( System.Uri url ).LocalPath
    with
      | ex -> url

  let createResult (hit:JsonProvider<"data/search/elasticResponseSchema.json">.Hit) =
    {
     Uri = chopPath hit.Source.Id;
     Abstract = hit.Source.HttpLdNiceOrgUkNsQualitystandardAbstract;
     Title = hit.Source.HttpLdNiceOrgUkNsQualitystandardTitle;
     FirstIssued = hit.Source.HttpLdNiceOrgUkNsQualitystandardFirstissued;
    }

  try
    let json = JsonProvider<"data/search/elasticResponseSchema.json">.Parse(response)
    printf "%A" json.Hits
    json.Hits.Hits |> Seq.map createResult |> Seq.toList
  with
    | ex ->
      printf "UNABLE TO PARSE ELASTIC SEARCH RESPONSE\n"
      []

let GetSearchResults runSearch testing query =
  query |> runSearch testing |> ParseResponse

let KnowledgeBaseCount testing =
  GetKBCount testing |> ParseCountResponse
