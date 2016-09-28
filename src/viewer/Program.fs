module Viewer.Program

open Suave
open Suave.Logging
open Suave.Web
open Suave.Http
open Viewer.SuaveExtensions
open Viewer.App
open Viewer.AppConfig
open System.Net
open System.Configuration
open Serilog
open NICE.Logging

let printAppSetting (key:string) =
  let value = ConfigurationManager.AppSettings.Get(key)
  printf "%s is %s\n" key value

[<EntryPoint>]
let main argv = 
  let mode = if argv.Length = 1 && argv.[0] = "dev" then Mode.Dev else Mode.Prod


  setTemplatesDir (System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/web/qs"))

  let defaultConfig = { defaultConfig with
                                      bindings = [ HttpBinding.mkSimple HTTP "0.0.0.0" 8083 ]
                                      homeFolder = Some (__SOURCE_DIRECTORY__ + "/web")}


  ["NiceLogging/AmqpUri"
   "NiceLogging/Environment"
   "NiceLogging/Application"] |> List.iter printAppSetting
  
  //printf "Running with server config:\n%A\n" defaultConfig
  Log.Logger <- LoggerConfiguration()
      .WriteTo.Nice()
        .CreateLogger()

  Log.Information("Starting up");

  let appConfig = getAppConfig mode
  startWebServer defaultConfig (createApp appConfig)

  0 
