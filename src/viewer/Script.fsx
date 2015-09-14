#r @"System.Net.Http"
#r "../../bin/viewer/Suave.dll"
#r "../../bin/viewer/DotLiquid.dll"
#r "../../bin/viewer/Suave.DotLiquid.dll"
open Suave
open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives
open Suave.Types

System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

#r @"../../packages/Elasticsearch.Net/lib/Elasticsearch.Net.dll"

open Elasticsearch.Net
open Elasticsearch.Net.Connection
open System

let uri = new Uri("http://localhost:9200")
let settings = new ConnectionConfiguration(uri).ExposeRawResponse()
let client = new ElasticsearchClient(settings)
let index = "kb"
let query = "{}"
let resp = client.Search<string>(index, query)
