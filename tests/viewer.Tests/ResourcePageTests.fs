module Viewer.Tests.ResourcePageTests

open Suave
open Suave.DotLiquid
open NUnit.Framework
open Swensen.Unquote
open Viewer.Tests.Utils
open System.IO

let writeToFile dir file content =
  try
    File.Delete(dir + file)
  with ex -> ()
  Directory.CreateDirectory(dir) |> ignore // fine if it already exists!

  File.WriteAllText(dir + file, content)

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "templates/"

[<Test>]
let ``Viewing a resource should load html file content`` () =

  writeToFile "/artifacts/published/qualitystandards/test_standard/test_statement/" "Statement.html" """<div id="content">Some content here</div>"""

  let content =
   startServer ()
   |> get "/qualitystandards/test_standard/test_statement/Statement.html"
   |> CQ.select "#content"
   |> CQ.text

  File.Delete("/artifacts/published/qualitystandards/test_standard/test_statement/Statement.html")

  test <@ content = "Some content here" @>

[<Test>]
let ``Viewing a resource should load contain disclaimer banner`` () =

  writeToFile "/artifacts/published/qualitystandards/test_standard/test_statement/" "Statement.html" ""

  let banner =
    startServer ()
    |> get "/qualitystandards/test_standard/test_statement/Statement.html"
    |> CQ.select ".banner"

  File.Delete("/artifacts/published/qualitystandards/test_standard/test_statement/Statement.html")

  test <@ banner |> CQ.length = 1 @>
