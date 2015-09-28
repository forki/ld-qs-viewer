module Viewer.Tests.ArtifactPageTests

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
let ``Viewing an artifact should load html file`` () =

  writeToFile "/artifacts/published/some/path/to/" "file.html" """<div id="content">Some content here</div>"""

  let content =
   startServer ()
   |> get "/some/path/to/file.html"
   |> CQ.select "#content"
   |> CQ.text 

  File.Delete("/artifacts/published/some/path/to/file.html")

  test <@ content = "Some content here" @>
