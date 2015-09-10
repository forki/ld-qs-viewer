open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Viewer.App

[<EntryPoint>]
let main argv =
  let templatePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/templates")
  setTemplatesDir templatePath
  let defaultConfig = { defaultConfig with bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ] }
  startWebServer defaultConfig app
  1
