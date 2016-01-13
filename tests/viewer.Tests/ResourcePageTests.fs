module Viewer.Tests.ResourcePageTests

open Suave
open Suave.DotLiquid
open Fuchu
open Swensen.Unquote
open Viewer.Tests.Utils
open System.IO

let writeToFile dir file content =
  try
    File.Delete(dir + file)
  with ex -> ()
  Directory.CreateDirectory(dir) |> ignore // fine if it already exists!

  File.WriteAllText(dir + file, content)

[<Tests>]
let tests =
  setTemplatesDir "src/viewer/bin/Release/"

  testList "Resource page" [

    testCase "Viewing a resource should load html file content" <| fun _ ->
      writeToFile "/artifacts/published/qualitystandards/test_standard/test_statement/" "Statement.html" """<div id="content">Some content here</div>"""

      let content =
       startServerWith baseConfig
       |> get "/qualitystandards/test_standard/test_statement/Statement.html"
       |> CQ.select "#resource-content"
       |> CQ.text

      File.Delete("/artifacts/published/qualitystandards/test_standard/test_statement/Statement.html")

      test <@ content.Contains "Some content here" @>

    testCase "Viewing a resource should load contain disclaimer banner" <| fun _ ->
      writeToFile "/artifacts/published/qualitystandards/test_standard/test_statement/" "Statement.html" ""

      let banner =
        startServerWith baseConfig
        |> get "/qualitystandards/test_standard/test_statement/Statement.html"
        |> CQ.select ".banner"

      File.Delete("/artifacts/published/qualitystandards/test_standard/test_statement/Statement.html")

      test <@ banner |> CQ.length = 1 @>

    testCase "Requesting a resource that does not exist should show message" <| fun _ ->
      let content =
        startServerWith baseConfig
        |> get "/qualitystandards/this/does/not/exist/Statement.html"
        |> CQ.select "#resource-content"
        |> CQ.text

      test <@ content.Contains "Could not find resource." @>
  ] 
