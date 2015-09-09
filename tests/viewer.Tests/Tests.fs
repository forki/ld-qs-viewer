module Viewer.Tests

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open NUnit.Framework
open Viewer.App

[<Test>]
let ``Should get Hello World`` () =
  let res =
    runWith defaultConfig app 
    |> req HttpMethod.GET "/" None

  Assert.AreEqual(res, "Hello World")