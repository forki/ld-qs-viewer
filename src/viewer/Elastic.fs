module Viewer.Elastic

open Viewer.Types
open Elasticsearch.Net
open Elasticsearch.Net.Connection
open System
open FSharp.Data

let BuildQuery qs =
  let query = """{
"from": 0, "size": 100,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "should" : [
          {"term" : {"qualitystandard:%s" : "%s"}}
        ]
      }
    }
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
