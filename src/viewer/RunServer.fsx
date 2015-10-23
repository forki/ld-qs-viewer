#r "DotLiquid.dll"
#r "Elasticsearch.Net.dll"
#r "FsPickler.dll"
#r "Nest.dll"
#r "Newtonsoft.Json.dll"
#r "Suave.dll"
#r "Suave.DotLiquid.dll"
#r "viewer.dll"
#r "FSharp.RDF.dll"

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Viewer.App
open Viewer.Types
open Viewer.Elastic
open Viewer.VocabGeneration
open FSharp.RDF

let devMode = fsi.CommandLineArgs.Length = 2 && fsi.CommandLineArgs.[1] = "dev"

let stubbedVocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/setting")
                                   Label = "Settings:"
                                   Selected = false
                                   Children = [
                                                Term { Uri = Uri.from "http://testing.com/TestSetting1"
                                                       Label = "Term1"
                                                       Selected = false
                                                       Children = []};
                                                Term { Uri = Uri.from "http://testing.com/TestSetting2"
                                                       Label = "Term2"
                                                       Selected = false
                                                       Children = []};]};
                     Property = "setting"}]

let getStubbedSearchResults _ _ = [{Uri = "Uri1"; Abstract = "Unicorns under the age of 65..."; Title = "This is the title (qs7)"};
                                   {Uri = "Uri2"; Abstract = "Goblins with arthritis..."; Title = "This is the title (qs7)"}]

let getSearchFunc () =
  match devMode with
    | true ->
      printf "RUNNING DEV MODE: Using stubbed data\n"
      getStubbedSearchResults
    | false -> GetSearchResults RunElasticQuery

let getVocabs () =
  match devMode with
    | true ->
      printf "RUNNING DEV MODE: Using stubbed data\n"
      stubbedVocabs
    | false -> readVocabsFromFiles ()
 

let templatePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/templates")
setTemplatesDir templatePath
let defaultConfig = { defaultConfig with
                                    bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ]
                                    homeFolder = Some (__SOURCE_DIRECTORY__ + "/web")
                    }

printf "Running with config:\n%A\n" defaultConfig
startWebServer defaultConfig (createApp (getVocabs()) (getSearchFunc()))
