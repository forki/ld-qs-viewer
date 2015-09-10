module Viewer.App

open Suave
open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives

type Annotation = {
  Name : string
  Uri : string
  }

type Annotations = {
  Settings : Annotation list
  AgeGroups : Annotation list
  }

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

let createApp getSettings getAgeGroups =
  //let xsSettings =
  //  getSettings
  //  |> Seq.map(fun (name, uri) -> { Name = name; Uri=uri })
  //  |> Seq.toList

  //let xsAgeGroups =
  //  getSettings
  //  |> Seq.map(fun (name, uri) -> { Name = name; Uri=uri })
  //  |> Seq.toList

  let model = {Settings = [{Name="1";Uri="1"}];AgeGroups = [{Name="2";Uri="2"}]}

  choose
    [ GET >>= choose
          //[path "/" >>= DotLiquid.page "home.html" ({Settings = xsSettings;AgeGroups = xsAgeGroups})]]
          [path "/" >>= DotLiquid.page "home.html" (model)]]
