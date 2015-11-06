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

let getSearchFunc () =
  match devMode with
    | true ->
      printf "RUNNING DEV MODE: Using stubbed data\n"
      Stubs.getSearchResults
    | false -> GetSearchResults RunElasticQuery

let getVocabs () =
  match devMode with
    | true ->
      printf "RUNNING DEV MODE: Using stubbed data\n"
      Stubs.vocabs
    | false -> readVocabsFromFiles ()

let getKBCountFunc () =
  match devMode with
    | true ->
      printf "RUNNING DEV MODE: Using empty KB Count"
      Stubs.getKBCount
    | false ->
      KnowledgeBaseCount


let templatePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/templates")
setTemplatesDir templatePath
let defaultConfig = { defaultConfig with
                                    bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ]
                                    homeFolder = Some (__SOURCE_DIRECTORY__ + "/web")}

printf "Running with config:\n%A\n" defaultConfig
startWebServer defaultConfig (createApp {Vocabs=getVocabs()
                                         GetSearchResults=getSearchFunc()
                                         GetKBCount=getKBCountFunc()})
