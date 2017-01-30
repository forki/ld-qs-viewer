module Viewer.Data.Search.Elastic

open Viewer.Utils
open Viewer.Types
open Viewer.Data.Search.Queries
open System
open FSharp.Data
open Serilog
open NICE.Logging

let BuildQuery filters =
  
  let shouldQuery = 
    filters
    |> Seq.filter(fun (x) -> not(x.Vocab.Equals("relevancyTest"))) 
    |> Seq.map (fun {Vocab=v; TermUris=terms} -> 
                    terms
                    |> Seq.map (fun t -> insertItemsInto termQuery (Uri.UnescapeDataString v) t)
                    |> concatToStringWithDelimiter ",")
    |> Seq.map (fun termQueriesStr -> insertItemInto shouldQuery termQueriesStr)
    |> concatToStringWithDelimiter ","

  let fullQuery = insertItemInto mustQuery shouldQuery
  printf "\n\nFull query without relevancy ->\n\n%A" fullQuery

  fullQuery

let BuildQueryWithRelevancy filters =
  
  let shouldQuery =
    filters
    |> Seq.filter(fun (x) -> not(x.Vocab.Equals("relevancyTest"))) 
    |> Seq.map (fun {Vocab=v; TermUris=terms} ->
                    terms
                    |> Seq.map (fun t -> insertItemsMultipleInto relevancyTermQuery (Uri.UnescapeDataString v) t)
                    |> concatToStringWithDelimiter ",")
    |> Seq.map (fun termQueriesStr -> insertItemInto shouldQuery termQueriesStr)
    |> concatToStringWithDelimiter ","

  let fullQuery = insertItemInto relevancyQuery shouldQuery
  printf "\n\nfull query->\n\n%A" fullQuery
  
  fullQuery

let GetKBCount testing =
  let indexName =
    match testing with
    | true -> "kb_test"
    | false -> "kb"
  printf "INDEX NAME-> %s" indexName
  let url = sprintf "http://elastic:9200/%s/_count?" indexName
  try
    Http.RequestString(url)
  with
    ex ->
      printf "ELASTICSEARCH_ERROR - unable to get count check index\n"
      ""

let RunElasticQuery testing (query: string) =
  let indexName =
    match testing with
    | true -> "kb_test"
    | false -> "kb"

  let url = sprintf "http://elastic:9200/%s/qualitystatement/_search?" indexName
  try
    Http.RequestString(url,
                       body = TextRequest query,
                       headers = [ "Content-Type", "application/json;charset=utf-8" ])
  with
    | ex ->
      Log.Error "ELASTICSEARCH_ERROR - unable to execute query%s\n"
      ""

let ParseCountResponse resp =
  try
    let json = JsonProvider<"data/search/elasticCountSchema.json">.Parse(resp)
    json.Count
  with
    | ex ->
      Log.Error "ELASTICSEARCH_ERROR - unable to parse response for count\n"
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
      Abstract = hit.Source.HttpsNiceOrgUkOntologiesQualitystandard1efaaa6aC81a4bd6B598C626b21c71fd
      Title = hit.Source.HttpsNiceOrgUkOntologiesQualitystandardBc8e0db05d8a410098f6774ac0eb1758
      FirstIssued = hit.Source.HttpsNiceOrgUkOntologiesQualitystandard0886da592c5f41249f466be4537a4099
    }

  try
    let json = JsonProvider<"data/search/elasticResponseSchema.json">.Parse(response)
    json.Hits.Hits |> Seq.map createResult |> Seq.toList
  with
    | ex ->
      let errorDump = sprintf "Message: %s\n Response: %s\n Exception: %s\n"  "UNABLE TO PARSE ELASTIC SEARCH RESPONSE" response (ex |> formatDisplayMessage) 
      Log.Error errorDump
      []

let KnowledgeBaseCount testing =
  GetKBCount testing |> ParseCountResponse

let searchWithRelevancy : (AggregatedFilter list -> SearchResult list) =
  BuildQueryWithRelevancy >> RunElasticQuery false >> ParseResponse
  
let search : (AggregatedFilter list -> SearchResult list) =
  BuildQuery >> RunElasticQuery false >> ParseResponse
