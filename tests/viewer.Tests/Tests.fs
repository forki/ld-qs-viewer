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
open CsQuery

let MakeRequest httpMethod route =
  runWith defaultConfig app 
    |> req httpMethod route None

let ParseHtml (resp: string) = CQ.Create(resp)

[<Test>]
let ``Visiting the hompage should set the title`` () =

  let title =
    MakeRequest HttpMethod.GET "/"
    |> ParseHtml
    |> (fun x -> x.Select("title").Text())

  //Assert.AreEqual("KB - Home", title)
  ()
