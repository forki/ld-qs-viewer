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
open Viewer.Tests.Stubs
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
  let GetSearchResults () = [{Uri = "Result1"};
                             {Uri = "Result2"}]
  let GetVocabularies = []

  let results =
    startServerWithData GetVocabularies GetSearchResults
    |> reqQuery HttpMethod.GET "/search" "q=1" 
    |> ParseHtml
    |> (fun x -> x.Select(".result"))

  test <@ results.Length = 2 @>
