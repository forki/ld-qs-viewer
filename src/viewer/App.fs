module Viewer.App

open Suave
open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives

type Annotation = {
  Name : string
  Uri : string
  }

type List = {
  Annotations : Annotation list
  }

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

let createApp fAnnotations =
  let xsAnnotations =
    fAnnotations
    |> Seq.map(fun (name, uri) -> { Name = name; Uri=uri })
    |> Seq.toList
  choose
    [ GET >>= choose
          [path "/" >>= DotLiquid.page "home.html" ({Annotations = xsAnnotations})]]
