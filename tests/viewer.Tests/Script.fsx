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

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open NUnit.Framework
open CsQuery
open Viewer.Tests
open Suave.DotLiquid

DotLiquid.setTemplatesDir("bin/Release/templates/")

type VocabularyTerm = {
                 Name: string
                 Uri: string
               }

type Model = {
  Settings: VocabularyTerm list
  AgeGroups: VocabularyTerm list
  }

let settings = [{Name = "Term1"; Uri = "Uri1"}]
let ageGroups = [{Name = "Term2"; Uri = "Uri2"}]
  
let model = {Settings = settings; AgeGroups = ageGroups}

let MakeRequest httpMethod route model = 
  runWith defaultConfig (DotLiquid.page "home.html" (model) )
    |> req httpMethod route None


let html = MakeRequest HttpMethod.GET "/" model 

