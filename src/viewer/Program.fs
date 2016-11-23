module Viewer.Program

open System
open Suave
open Suave.Logging
open Suave.Web
open Suave.Http
open Viewer.SuaveExtensions
open Viewer.App
open Viewer.AppConfig
open Viewer.SuaveSerilogAdapter
open System.Net
open System.Configuration
open Serilog
open NICE.Logging
open Viewer.Types

let printAppSetting (key:string) =
  let value = ConfigurationManager.AppSettings.Get(key)
  printf "%s is %s\n" key value

let customErrorHandler (ex : Exception) msg (ctx : HttpContext) =
  Log.Error("Error {@message}, {@exception}", msg, ex)
  ServerErrors.INTERNAL_ERROR "Looks like our server got itself in trouble. We've logged it and will investigate. Sorry for the inconvenience - press the back button in your browser." ctx

[<EntryPoint>]
let main argv = 
  let mode = if argv.Length = 1 && argv.[0] = "dev" then Mode.Dev else Mode.Prod

  ["NiceLogging/AmqpUri"
   "NiceLogging/Environment"
   "NiceLogging/Application"] |> List.iter printAppSetting

  let logConfig = 
    match mode with
    | Dev -> LoggerConfiguration().WriteTo.Nice().WriteTo.Console()
    | Prod -> LoggerConfiguration().WriteTo.Nice()

  Log.Logger <- logConfig.MinimumLevel.Debug().CreateLogger()

  let rootDir = (System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/web"))
  setTemplatesDir (System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/web/qs"))

  let OntologyConfig = { TtlRoot= "http://schema/ontologies/"
                         CoreTtl= "qualitystandard.ttl"
                         ContextLabel= "qualitystandard"
                         ContextValue= "https://nice.org.uk/ontologies/qualitystandard/"
                         Predicates = [
                            { Uri= "qualitystandard:62496684_7027_4f37_bd0e_264c9ff727fd"; SourceTtl= "setting.ttl" }
                            { Uri= "qualitystandard:4e7a368e_eae6_411a_8167_97127b490f99"; SourceTtl= "agegroup.ttl" }
                            { Uri= "qualitystandard:7ae8413a_2811_4a09_a655_eff8d276ec87"; SourceTtl= "servicearea.ttl" }
                            { Uri= "qualitystandard:18aa6468_de94_4f9f_bd7a_0075fba942a5"; SourceTtl= "factorsaffectinghealthorwellbeing.ttl" }
                            { Uri= "qualitystandard:28745bc0_6538_46ee_8b71_f0cf107563d9"; SourceTtl= "conditionordisease.ttl" }
                         ]
                       }

  
  let defaultConfig = { defaultConfig with
                                      bindings = [ HttpBinding.mkSimple HTTP "0.0.0.0" 8083 ]
                                      homeFolder = Some rootDir
                                      logger = SuaveSerilogAdapter Log.Logger
                                      errorHandler = customErrorHandler}

  
  //printf "Running with server config:\n%A\n" defaultConfig

  let appConfig = getAppConfig mode
  startWebServer defaultConfig (createApp appConfig)

  0 
