open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Viewer.App
open Viewer.Types

[<EntryPoint>]
let main argv =

  let vocabularies = {Vocabularies = [{Name = "Settings";
                                       Terms = [{Name = "Care Home"; Uri = "http://ld.nice.org.uk/ns/Care_Home"};
                                                {Name = "Hospital"; Uri = "http://ld.nice.org.uk/ns/Hospital"}]};
                                      {Name = "Age Groups";
                                       Terms = [{Name = "Older Adult"; Uri = "http://ld.nice.org.uk/ns/Hospital"}
                                                {Name = "Adult less than 65"; Uri = "http://ld.nice.org.uk/ns/Hospital"}]}]};

  let templatePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "bin/viewer/templates")
  setTemplatesDir templatePath
  let defaultConfig = { defaultConfig with bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ] }
  startWebServer defaultConfig (createApp vocabularies)
  1
