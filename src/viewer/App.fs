module Viewer.App

open Suave.Http.Successful
open Suave.Http
open Suave.Http.Applicatives

let app =
  choose
    [ GET >>= choose
          [path "/" >>= OK "Hello World" ]]
