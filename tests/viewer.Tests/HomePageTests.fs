module Viewer.Tests.HomePageTests

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open NUnit.Framework
open Swensen.Unquote
open Viewer.App
open Viewer.Types
open Viewer.Tests.Utils

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "templates/"

[<Test>]
let ``Should set the title`` () =
  let title =
    startServer ()
    |> req HttpMethod.GET "/" None
    |> ParseHtml
    |> (fun x -> x.Select("title").Text())
  test <@ title = "KB - Home" @>

[<Test>]
let ``Should show heading`` () =
  let header =
    startServer ()
    |> req HttpMethod.GET "/" None
    |> ParseHtml
    |> (fun x -> x.Select("main > h1").Text())
  test <@ header = "NICE Quality Standards" @>

[<Test>]
let ``Should add form with search action`` () =
  let form =
    startServer ()
    |> req HttpMethod.GET "/" None
    |> ParseHtml
    |> (fun x -> x.Select("form"))
  test <@  form.Attr("action") = "/search" @>

[<Test>]
let ``Should present the vocabulary terms in form`` () =
  let vocabularies = [{Name = "Vocab 1";
                       Terms = [{Name = "Term1"; Uri = "Uri1"};
                                {Name = "Term2"; Uri = "Uri2"}]};
                      {Name = "Vocab 2";
                       Terms = [{Name = "Term3"; Uri = "Uri3"}]}]

  let html =
    startServerWithData vocabularies NoSearchResults
    |> req HttpMethod.GET "/" None
    |> ParseHtml

  let vocab1 = html.Select("form > .vocab").First()
  test <@ vocab1.Text().StartsWith("Vocab 1") @>

  let vocab2 = html.Select("form > .vocab").Last()
  test <@ vocab2.Text().StartsWith("Vocab 2") @>

  let terms = html.Select("form > .vocab > input")
  test <@ terms.Length = 3 @>

[<Test>]
let ``Should have search button`` () =
  let searchbutton =
    startServer ()
    |> req HttpMethod.GET "/" None
    |> ParseHtml
    |> (fun x -> x.Select(":submit"))
  test <@ searchbutton.Attr("Value") = "Search" @>
