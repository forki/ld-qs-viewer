#r @"System.Net.Http"
#r "../../bin/viewer/viewer.dll"
#r "bin/Release/Unquote.dll"
open Viewer.SuaveExtensions
open Viewer.Components.Hotjar

setTemplatesDir "src/viewer/bin/Release/"

let res = template "components/hotjar/index.html" {Id = "12345"}
test <@ res = "arse" @>


let uri = ""
