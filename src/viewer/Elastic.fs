module Viewer.Elastic

open Viewer.Types
open Elasticsearch.Net
open Elasticsearch.Net.Connection
open System
open FSharp.Data

let BuildQuery qs =

  let buildTermListFromQueryString qs =
    
    let buildTerm k v =
      let termQuery = """{"term" : {"qualitystandard:%s" : "%s"}}"""
      sprintf (Printf.StringFormat<string->string->string>(termQuery)) k v

    qs
    |> Seq.map (fun pair ->
                match pair with
                  | (k, Some(v)) -> buildTerm k v)
    |> Seq.toList

  let buildTermQueryFromTerms terms = 
    terms
    |> Seq.fold (fun acc term ->
                 match acc with
                   | "" -> term
                   | _ -> acc + "," + term) ""

  let insertIntoQuery query termQuery = 
    sprintf (Printf.StringFormat<string->string>(query)) termQuery

  let query = """{
"from": 0, "size": 100,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "should" : [
          %s
        ]
      }
    }
  }
}
}"""

  let fullQuery = qs
                |> buildTermListFromQueryString
                |> buildTermQueryFromTerms
                |> insertIntoQuery query

  //printf "Running query: %s" fullQuery
  fullQuery

let RunElasticQuery testing (query: string) =
  let indexName = 
    match testing with
    | true -> "kb_test"
    | false -> "kb"

  let url = sprintf "http://elastic:9200/%s/qualitystatement/_search?" indexName
  Http.RequestString(url, body = TextRequest query)

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

let GetSearchResults runSearch testing query =
  query |> runSearch testing |> ParseResponse
