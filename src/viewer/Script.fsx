#r @"System.Net.Http"
#r "../../bin/viewer/Suave.dll"
#r "../../bin/viewer/DotLiquid.dll"
#r "../../bin/viewer/Suave.DotLiquid.dll"
#r @"../../packages/Elasticsearch.Net/lib/Elasticsearch.Net.dll"
#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.DesignTime.dll"
open Suave
open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives
open Suave.Types
open Elasticsearch.Net
open Elasticsearch.Net.Connection
open System
open FSharp.Data

System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

open System
S

//let uri = new Uri("http://localhost:9200")
//let settings = new ConnectionConfiguration(uri) |> (fun x -> x.ExposeRawResponse())
//let client = new ElasticsearchClient(settings) 
//let index = "kb"
//let query = "{}"
//let resp = client.Search<string>(index, query)
//let raw = resp.Response

type jp = JsonProvider<"../../src/viewer/elasticResponseSchema.json">
let s = jp.GetSample()
let hits = s.Hits.Hits
hits
  |> Seq.map(fun hit -> hit.Source.QualitystandardAbstract)
  |> Seq.toList


let url = "http://ld.nice.org.uk/prov/entity#98ead3d:qualitystandards/qs7/st2/Statement.md" 

let parts = url.Split (':')
let path = parts.[2]
let id = path.Split('.')
id.[0]


let l = [("A","B");("C","D");("A","E")]
l
|> Seq.groupBy (fun (k,v) -> k)
|> Seq.map (fun (k, v) -> (k, Seq.map (fun (s,p) -> p) v) )


let l = ["A";"B";"C"]
