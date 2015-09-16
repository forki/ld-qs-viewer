module Viewer.Elastic

open Viewer.Types
open Elasticsearch.Net
open Elasticsearch.Net.Connection
open System
open FSharp.Data

let buildQuery qs =
  let query = 
    """
    {
       "query": {
            "term" : {
                "%s" : "%s"
            }
       }
    }"""

  qs |> Seq.head
  |> (fun a ->
      match a with
      | (k, Some(v)) -> (sprintf (Printf.StringFormat<string->string->string>(query)) k v))

let RunElasticQuery (query: string) =
  let req = "http://localhost:9200/kb/qs/_search?"
  Http.RequestString(req,
                     body = TextRequest query)



let GetSearchResults runSearchWith query =
  let queryResponse = runSearchWith query
  try
    let json = FSharp.Data.JsonProvider<"elasticResponseSchema.json">.Parse(queryResponse)
    let hits = json.Hits.Hits
    hits
      |> Seq.map(fun x -> {Uri = x.Source.Id})
      |> Seq.toList
  with
    | ex ->
      printf "%s" (ex.ToString())
      []
