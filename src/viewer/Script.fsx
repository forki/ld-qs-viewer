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


open System.Text.RegularExpressions
let path = "qualitystandards/qs7/st2/Statement.md" 

let m = Regex.Match(path,"qs([0-9]+)/st([0-9]+)")
let standard = m.Groups.[1].Value
let statement = m.Groups.[2].Value
printf "Quality Statement %s from Quality standard %s" statement standard
