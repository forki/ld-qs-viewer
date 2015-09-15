#I "bin/Release"
#r "bin/Release/CsQuery.dll"
#r "bin/Release/DotLiquid.dll"
#r "bin/Release/FsPickler.dll"
#r "bin/Release/Fuchu.dll"
#r "bin/Release/nunit.framework.dll"
#r "bin/Release/Suave.dll"
#r "bin/Release/Suave.DotLiquid.dll"
#r "bin/Release/Suave.Testing.dll"
#r "bin/Release/viewer.Tests.dll"
#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "../../packages/FSharp.Data/lib/net40/FSharp.Data.DesignTime.dll"
#load "Stubs.fs"

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open CsQuery
open Viewer.Tests
open Suave.DotLiquid
open Viewer.Tests.Stubs
open FSharp.Data

type jp = JsonProvider<"../../src/viewer/elasticResponseSchema.json">
let json = jp.Parse("""
{
  "hits":{
    "hits":[
      {
        "_id":"qs1_st1",
        "_source":{
          "@id":"http://ld.nice.org.uk/prov/entity#f178fc5:qualitystandards/qs1/st1/Statement.md",
          "abstract":"This is statement 1"
        }
      },
      {
        "_id":"qs1_st2",
        "_source":{
          "@id":"http://ld.nice.org.uk/prov/entity#f178fc5:qualitystandards/qs1/st2/Statement.md",
          "abstract":"This is statement 2"
        }
      }
    ]
  }
}
                    """)
json.Hits 

