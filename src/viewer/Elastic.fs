module Viewer.Elastic

open Viewer.Utils
open Viewer.Types
open Viewer.Queries
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
      printf "%s\n" (ex.ToString())
      ""

let RunElasticQuery testing (query: string) =
  let indexName =
    match testing with
    | true -> "kb_test"
    | false -> "kb"

  let url = sprintf "http://elastic:9200/%s/qualitystatement/_search?" indexName
  try
    Http.RequestString(url, body = TextRequest query)
  with
    | ex ->
      printf "%s\n" (ex.ToString())
      ""

let ParseCountResponse resp =
  try
    let json = JsonProvider<"elasticCountSchema.json">.Parse(resp)
    json.Count
  with
    | ex ->
      printf "%s\n" (ex.ToString())
      0

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

  let createResult (hit:JsonProvider<"elasticResponseSchema.json">.Hit) =
    {Uri = chopPath hit.Source.Id;
     Abstract = hit.Source.HttpLdNiceOrgUkNsQualitystandardAbstract;
     Title = hit.Source.HttpLdNiceOrgUkNsQualitystandardTitle}

  try
    let json = JsonProvider<"elasticResponseSchema.json">.Parse(response)
    json.Hits.Hits |> Seq.map createResult |> Seq.toList
  with
    | ex ->
      printf "%s\n" (ex.ToString())
      []

let GetSearchResults runSearch testing query =
  query |> runSearch testing |> ParseResponse

let KnowledgeBaseCount testing =
  GetKBCount testing |> ParseCountResponse
