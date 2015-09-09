module viewer.Tests

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open NUnit.Framework
open viewer

[<Test>]
let ``Should get Hello World`` () =
  let res =
    runWith defaultConfig (app :?> WebPart)
    |> req HttpMethod.GET "/" None
    
  Assert.AreEqual(res, "Hello World")
