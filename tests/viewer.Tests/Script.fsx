#load "LoadDeps.fsx"
open Viewer.SuaveExtensions
open Viewer.Components.Hotjar
open Swensen.Unquote

setTemplatesDir "../../src/viewer/bin/Release/"

let res = template "components/hotjar/index.html" {Id = "12345"}
test <@ res = "arse" @>

