#I "../../bin/viewer/"
#r @"System.Net.Http"
#r "../../bin/viewer/Suave.dll"
#r "../../bin/viewer/DotLiquid.dll"
#r "../../bin/viewer/Suave.DotLiquid.dll"
#r "../../packages/Suave.Testing/lib/net40/Suave.Testing.dll"

open Suave
open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives
open Suave.Testing
open Suave.Types

System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

DotLiquid.setTemplatesDir("./templates")

type VocabularyTerm = {
  Name: string
  Uri: string
  }

type Home  = {
  Ages: VocabularyTerm list
  Settings: VocabularyTerm list
  }

let settings = [{Name = "Term1"; Uri = "Uri1"}]
let ageGroups = [{Name = "Term2"; Uri = "Uri2"}]

let model = {Settings = settings; Ages = ageGroups}

let (Some html) = DotLiquid.page "home.html" (model) HttpContext.empty |> Async.RunSynchronously

match html.response.content with
  | Bytes(x) -> System.Text.UTF8Encoding.UTF8.GetString x
