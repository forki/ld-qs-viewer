module Viewer.Tests.SearchPageTests

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open Suave.DotLiquid
open NUnit.Framework
open Swensen.Unquote
open Viewer.App
open Viewer.Types
open Viewer.Tests.Utils

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "templates/"

[<Test>]
let ``Should present zero results when no query string provided`` () =
 let results =
   startServer ()
   |> get "/"
   |> CQ.select ".result"
   |> CQ.length

 test <@ results = 0 @>

[<Test>]
let ``Should present search results`` () =
  let GetSearchResults _ = [{Uri = "";Abstract = ""};
                            {Uri = "";Abstract = ""}]
  let GetVocabularies = []

  let results =
    startServerWithData GetVocabularies GetSearchResults
    |> getQuery "/search" "notused=notused"
    |> CQ.select ".results > .result"
    |> CQ.length

  test <@ results = 2 @>

[<Test>]
let ``Should present abstract and link for each result`` () =
  let GetSearchResults _ = [{Uri = "Uri1"; Abstract = "Abstract1"};
                            {Uri = "Uri2"; Abstract = "Abstract2"}]
  let GetVocabularies = []

  let dom =
    startServerWithData GetVocabularies GetSearchResults
    |> getQuery "/search" "notused=notused"

  let abstracts = dom |> CQ.select ".result > .abstract"

  let abstract1 = abstracts |> CQ.first |> CQ.text
  let abstract2 = abstracts |> CQ.last |> CQ.text

  test <@ abstract1 = "Abstract1" @>
  test <@ abstract2 = "Abstract2" @>

  let links = dom |> CQ.select ".result > a"
  let link1 = links |> CQ.first |> CQ.text
  let link2 = links |> CQ.last |> CQ.text

  test <@ link1 = "Uri1" @>
  test <@ link2 = "Uri2" @>
