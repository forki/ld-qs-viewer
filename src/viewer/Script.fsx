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

DotLiquid.setTemplatesDir("bin/Release/templates/")

type VocabularyTerm = {
  Name: string
  Uri: string
  }

type Home  = {
  Settings: VocabularyTerm list 
  AgeGroups: VocabularyTerm list 
  }

let settings = [{Name = "Term1"; Uri = "Uri1"}]
let ageGroups = [{Name = "Term2"; Uri = "Uri2"}]

let model = {Settings = settings; AgeGroups = ageGroups}

let (Some html) = DotLiquid.page "home.html" (model) HttpContext.empty |> Async.RunSynchronously
