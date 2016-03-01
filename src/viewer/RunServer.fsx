#I "../viewer/bin/Release"
#r "Suave.dll"
#r "viewer.dll"

open Suave
open Suave.Web
open Suave.Http
open Viewer.SuaveExtensions
open Viewer.App
open Viewer.AppConfig

let mode = if fsi.CommandLineArgs.Length = 2 && fsi.CommandLineArgs.[1] = "dev" then Mode.Dev else Mode.Prod


setTemplatesDir (System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer"))

let defaultConfig = { defaultConfig with
                                    bindings = [ HttpBinding.mkSimple HTTP "0.0.0.0" 8083 ]
                                    homeFolder = Some (__SOURCE_DIRECTORY__ + "/web")}

//printf "Running with server config:\n%A\n" defaultConfig
let appConfig = getAppConfig mode
startWebServer defaultConfig (createApp appConfig)
