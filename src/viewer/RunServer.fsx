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

let vocabularies = [{Name = "Settings";
                     Terms = [{Name = "Care Home"; Uri = "http://ld.nice.org.uk/ns/Care_Home"};
                              {Name = "Hospital"; Uri = "http://ld.nice.org.uk/ns/Hospital"}]};
                    {Name = "Age Groups";
                     Terms = [{Name = "Older Adult"; Uri = "http://ld.nice.org.uk/ns/Hospital"}
                              {Name = "Adult less than 65"; Uri = "http://ld.nice.org.uk/ns/Hospital"}]}]

let SearchFunc = GetSearchResults RunElasticQuery

let templatePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/templates")
setTemplatesDir templatePath
let defaultConfig = { defaultConfig with
                                    bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ]
                                    homeFolder = Some (__SOURCE_DIRECTORY__ + "/web")
                    }

printf "Running with config:\n%A" defaultConfig
startWebServer defaultConfig (createApp vocabularies SearchFunc)
