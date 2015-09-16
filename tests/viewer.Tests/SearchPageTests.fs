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
   |> req HttpMethod.GET "/search" None
   |> ParseHtml
   |> (fun x -> x.Select(".result"))

 test <@ results.Length = 0 @>

[<Test>]
let ``Should present search results`` () =
  let GetSearchResults _ = [{Uri = "";Abstract = ""};
                            {Uri = "";Abstract = ""}]
  let GetVocabularies = []

  let results =
    startServerWithData GetVocabularies GetSearchResults
    |> reqQuery HttpMethod.GET "/search" "q=1" 
    |> ParseHtml
    |> (fun x -> x.Select(".results > .result"))

  test <@ results.Length = 2 @>

[<Test>]
let ``Should present abstract and link for each result`` () =
  let GetSearchResults _ = [{Uri = "Uri1"; Abstract = "Abstract1"};
                            {Uri = "Uri2"; Abstract = "Abstract2"}]
  let GetVocabularies = []

  let dom =
    startServerWithData GetVocabularies GetSearchResults
    |> reqQuery HttpMethod.GET "/search" "q=1" 
    |> ParseHtml

  let abstracts = dom.Select(".result > .abstract")

  test <@ abstracts.First().Text() = "Abstract1" @>
  test <@ abstracts.Last().Text() = "Abstract2" @>

  let links = dom.Select(".result > a")

  test <@ links.First().Text() = "Uri1" @>
  test <@ links.Last().Text() = "Uri2" @>
