open Suave
open Suave.Http.Successful
open Suave.Web

[<EntryPoint>]
let main argv =
  startWebServer defaultConfig (OK "Hello World")
  1
