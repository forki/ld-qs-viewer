module Viewer.Elastic

open Viewer.Types
open Elasticsearch.Net
open Elasticsearch.Net.Connection
open System
open FSharp.Data

let aggregateQueryStringValues qs =
  qs
  |> Seq.groupBy (fun (k,_) -> k)
  |> Seq.map (fun (k, vals) ->
              (k, vals
                  |> Seq.map (fun (_,p) ->
                              match p with
                              | Some s -> s)
                  |> Seq.toList))
  |> Seq.toList

let BuildQuery qs =

  let buildTermListFromQueryString (k:string) vals =
    
    let buildTerm k v =
      let termQuery = """{"term" : {"qualitystandard:%s" : "%s"}}"""
      sprintf (Printf.StringFormat<string->string->string>(termQuery)) k v

    vals
    |> Seq.map (fun v -> buildTerm k v)

  let buildQueryFromList terms = 
    terms
    |> Seq.fold (fun acc term ->
                 match acc with
                   | "" -> term
                   | _ -> acc + "," + term) ""

  let shouldQuery = """{"bool" : {
            "should" : [
              %s
            ]
          }}"""

  let mustQuery = """{
"from": 0, "size": 100,
"query": {
  "filtered": {
    "filter" : {
      "bool" : {
        "must" : [
          %s
        ]
      } 
    }
  }
}
}"""

  let insertIntoShouldQuery (k:string) terms =
    let termQuery = terms |> buildTermListFromQueryString k |> buildQueryFromList
    sprintf (Printf.StringFormat<string->string>(shouldQuery)) termQuery

  let insertIntoMustQuery queries = 
    let shoulds = queries |> buildQueryFromList
    sprintf (Printf.StringFormat<string->string>(mustQuery)) shoulds

  let aggs = qs |> aggregateQueryStringValues
  let shouldQueries = aggs |> Seq.map (fun (k, vals) -> insertIntoShouldQuery k vals)
  let fullQuery = insertIntoMustQuery shouldQueries

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
