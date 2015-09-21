module Viewer.Elastic

open Viewer.Types
open Elasticsearch.Net
open Elasticsearch.Net.Connection
open System
open FSharp.Data

let BuildQuery qs =
  let query = 
    """
    {
       "query": {
            "term" : {
                "qualitystandard:%s" : "%s"
            }
       }
    }"""

  qs
  |> Seq.head
  |> (fun a ->
      match a with
      | (k, Some(v)) -> (sprintf (Printf.StringFormat<string->string->string>(query)) k v))

let RunElasticQuery (query: string) =
  Http.RequestString("http://elastic:9200/kb/qualitystatement/_search?", body = TextRequest query)

let ParseResponse response = 
  try
    let json = FSharp.Data.JsonProvider<"elasticResponseSchema.json">.Parse(response)
    let hits = json.Hits.Hits
    hits
      |> Seq.map(fun x -> {Uri = x.Source.Id;Abstract = x.Source.DctermsAbstract})
      |> Seq.toList
  with
    | ex ->
      printf "%s" (ex.ToString())
      []

let GetSearchResults runSearch query =
  query |> runSearch |> ParseResponse
