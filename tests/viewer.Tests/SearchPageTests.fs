module Viewer.Tests.SearchPageTests

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open NUnit.Framework
open Viewer.App
open Viewer.Tests.Stubs
open CsQuery

let ParseHtml (resp: string) = CQ.Create(resp)

let MakeRequest httpMethod route GetSearchResults =
  let GetVocabularies = []
  runWith defaultConfig (createApp GetVocabularies GetSearchResults)
    |> req httpMethod route None
    |> ParseHtml

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "templates/"

[<Test>]
let ``Should present zero results when no query string provided`` () =
 let GetSearchResults () = ""

 let results =
   MakeRequest HttpMethod.GET "/search" GetSearchResults
    |> (fun x -> x.Select("#result"))

 Assert.AreEqual(0, results.Length)

//[<Test>]
//let ``Should present search results`` () =
//  let GetSearchResults () = stubbedElasticResponse
//  let results =
//    MakeRequest HttpMethod.GET "/search?q=1" GetSearchResults
//    |> (fun x -> x.Select("#result"))
//
//  Assert.AreEqual(2, results.Length)
