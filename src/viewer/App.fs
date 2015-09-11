module Viewer.App

open Suave
open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives
open Viewer.Types

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

type HomeModel =  {
   Vocabularies: Vocabulary list
 }

let createApp vocabularies =
  choose
    [ GET >>= choose
          [path "/" >>= DotLiquid.page "home.html" {Vocabularies = vocabularies}]]
