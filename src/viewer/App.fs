module Viewer.App

open Suave
open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives

let setTemplatesDir path = 
  DotLiquid.setTemplatesDir(path)

let app =
  choose
    [ GET >>= choose
          [path "/" >>= DotLiquid.page "home.html" () ]]
