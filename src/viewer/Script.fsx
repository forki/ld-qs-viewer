#I "bin/Release"
#r "bin/Release/Suave.dll"

open Suave
open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives

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

let html = DotLiquid.page "home.html" (model)

let HttpContext t = match html with
  | Some x -> x
  | None -> "none" 

