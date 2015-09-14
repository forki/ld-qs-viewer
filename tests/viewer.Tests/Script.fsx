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

DotLiquid.setTemplatesDir("bin/Release/templates/")

type jp = JsonProvider<"../../elasticSchema.json">
let json = jp.GetSample()







