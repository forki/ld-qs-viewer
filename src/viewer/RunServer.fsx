#r "DotLiquid.dll"
#r "Elasticsearch.Net.dll"
#r "FsPickler.dll"
#r "Nest.dll"
#r "Newtonsoft.Json.dll"
#r "Suave.dll"
#r "Suave.DotLiquid.dll"
#r "viewer.dll"

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Viewer.App
open Viewer.Types
open Viewer.Elastic

let devMode = fsi.CommandLineArgs.Length = 2 && fsi.CommandLineArgs.[1] = "dev"

let stubbedVocabularies = [{Name = "setting";
                     Terms = [{Name = "Hospice"; Uri = "http://ld.nice.org.uk/ns/qualitystandard/setting#Hospice"};
                              {Name = "Community"; Uri = "http://ld.nice.org.uk/ns/qualitystandard/setting#Community"}]};]

let getStubbedSearchResults _ = [{Uri = "http://localhost/resource/FHSJAJWHEHFK"; Abstract = "Unicorns under the age of 65..."};
                                 {Uri = "http://localhost/resource/AWEKSJDJJJSEJ"; Abstract = "Goblins with arthritis..."}]

let getSearchFunc () =
  match devMode with
    | true ->
      printf "RUNNING DEV MODE: Using stubbed data\n"
      getStubbedSearchResults
    | false -> GetSearchResults RunElasticQuery

let templatePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/templates")
setTemplatesDir templatePath
let defaultConfig = { defaultConfig with
                                    bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ]
                                    homeFolder = Some (__SOURCE_DIRECTORY__ + "/web")
                    }

printf "Running with config:\n%A\n" defaultConfig
startWebServer defaultConfig (createApp stubbedVocabularies (getSearchFunc()))
