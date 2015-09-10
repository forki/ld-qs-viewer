open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Viewer.App

[<EntryPoint>]
let main argv =

  let getSettings =
    [("Care Home", "http://ld.nice.org.uk/ns/Care_Home");
     ("Hospital", "http://ld.nice.org.uk/ns/Hospital")]

  let getAgeGroups =
    [("Older Adult", "http://ld.nice.org.uk/ns/Care_Home");
     ("Adult less than 65", "http://ld.nice.org.uk/ns/Hospital")]

  let templatePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/templates")
  setTemplatesDir templatePath
  let defaultConfig = { defaultConfig with bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ] }
  startWebServer defaultConfig (createApp getSettings getAgeGroups)
  1
