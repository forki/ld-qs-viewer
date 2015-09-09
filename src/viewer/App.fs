module Viewer.App

open Suave
open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives


let app =
  choose
    [ GET >>= choose
          [path "/" >>= DotLiquid.page "home.html" () ]]
