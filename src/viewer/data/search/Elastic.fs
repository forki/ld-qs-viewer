module Viewer.Data.Search.Elastic

open Viewer.Utils
open Viewer.Types
open Viewer.Data.Search.Queries
open System
open FSharp.Data
open System.Text.RegularExpressions
open System.Reflection
open System.Text
open Microsoft.FSharp.Core.Printf

let formatDisplayMessage (e:Exception) =
    let sb = StringBuilder()
    let delimeter = String.replicate 50 "*"
    let nl = Environment.NewLine
    let rec printException (e:Exception) count =
        if (e :? TargetException && e.InnerException <> null)
        then printException (e.InnerException) count
        else
            if (count = 1) then bprintf sb "%s%s%s" e.Message nl delimeter
            else bprintf sb "%s%s%d)%s%s%s" nl nl count e.Message nl delimeter
            bprintf sb "%sType: %s" nl (e.GetType().FullName)
            // Loop through the public properties of the exception object
            // and record their values.
            e.GetType().GetProperties()
            |> Array.iter (fun p ->
                // Do not log information for the InnerException or StackTrace.
                // This information is captured later in the process.
                if (p.Name <> "InnerException" && p.Name <> "StackTrace" &&
                    p.Name <> "Message" && p.Name <> "Data") then
                    try
                        let value = p.GetValue(e, null)
                        if (value <> null)
                        then bprintf sb "%s%s: %s" nl p.Name (value.ToString())
                    with
                    | e2 -> bprintf sb "%s%s: %s" nl p.Name e2.Message
            )
            if (e.StackTrace <> null) then
                bprintf sb "%s%sStackTrace%s%s%s" nl nl nl delimeter nl
                bprintf sb "%s%s" nl e.StackTrace
            if (e.InnerException <> null)
            then printException e.InnerException (count+1)
    printException e 1
    sb.ToString()

let BuildQuery filters =
  
  let shouldQuery = 
    filters
    |> Seq.map (fun {Vocab=v; TermUris=terms} -> 
                    terms
                    |> Seq.map (fun t -> insertItemsInto termQuery (Uri.UnescapeDataString v) t)
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
     Abstract = hit.Source.HttpsNiceOrgUkOntologiesQualitystandardAbstract;
     Title = hit.Source.HttpsNiceOrgUkOntologiesQualitystandardTitle;
     FirstIssued = hit.Source.HttpsNiceOrgUkOntologiesQualitystandardWasFirstIssuedOn;
    }

  try
    let json = JsonProvider<"data/search/elasticResponseSchema.json">.Parse(response)
    json.Hits.Hits |> Seq.map createResult |> Seq.toList
  with
    | ex ->
      printf "%s" (ex|>formatDisplayMessage) 
      printf "UNABLE TO PARSE ELASTIC SEARCH RESPONSE\n"
      []

let KnowledgeBaseCount testing =
  GetKBCount testing |> ParseCountResponse

let search : (AggregatedFilter list -> SearchResult list) =
  BuildQuery >> RunElasticQuery false >> ParseResponse
