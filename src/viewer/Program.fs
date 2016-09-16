module Viewer.Program

open Suave
open Suave.Logging
open Suave.Web
open Suave.Http
open Viewer.SuaveExtensions
open Viewer.App
open Viewer.AppConfig
open System.Net
open Serilog
open NICE.Logging


[<EntryPoint>]
let main argv = 
  let mode = if argv.Length = 1 && argv.[0] = "dev" then Mode.Dev else Mode.Prod


  setTemplatesDir (System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/web/qs"))

  let defaultConfig = { defaultConfig with
                                      bindings = [ HttpBinding.mkSimple HTTP "0.0.0.0" 8083 ]
                                      homeFolder = Some (__SOURCE_DIRECTORY__ + "/web")}

  //printf "Running with server config:\n%A\n" defaultConfig
  Log.Logger <- LoggerConfiguration()
      .WriteTo.Nice()
        .CreateLogger()
  Log.Information("blah");
  let appConfig = getAppConfig mode
  startWebServer defaultConfig (createApp appConfig)

  0 
