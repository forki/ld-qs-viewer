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

let vocabularies = [{Name = "settings";
                     Terms = [{Name = "Hospice"; Uri = "http://ld.nice.org.uk/ns/qualitystandard/setting#Hospice"};
                              {Name = "Community"; Uri = "http://ld.nice.org.uk/ns/qualitystandard/setting#Community"}]};]

let SearchFunc = GetSearchResults RunElasticQuery

let templatePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/templates")
setTemplatesDir templatePath
let defaultConfig = { defaultConfig with
                                    bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ]
                                    homeFolder = Some (__SOURCE_DIRECTORY__ + "/web")
                    }

printf "Running with config:\n%A" defaultConfig
startWebServer defaultConfig (createApp vocabularies SearchFunc)
