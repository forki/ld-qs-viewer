module Viewer.App

open Suave
open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives
open Viewer.Types

type Model = {
  Settings: VocabularyTerm list
  AgeGroups: VocabularyTerm list
  }

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

let createApp vocabularies =
  //let xsSettings =
  //  getSettings
  //  |> Seq.map(fun (name, uri) -> { Name = name; Uri=uri })
  //  |> Seq.toList

  //let xsAgeGroups =
  //  getSettings
  //  |> Seq.map(fun (name, uri) -> { Name = name; Uri=uri })
  //  |> Seq.toList
  
  let settings = [{Name = "Term1"; Uri = "Uri1"}]
  let ageGroups = [{Name = "Term2"; Uri = "Uri2"}]
  
  let model = {Settings = settings; AgeGroups = ageGroups}

  choose
    [ GET >>= choose
          //[path "/" >>= DotLiquid.page "home.html" ({Settings = xsSettings;AgeGroups = xsAgeGroups})]]
          [path "/" >>= DotLiquid.page "home.html" (model)]]
