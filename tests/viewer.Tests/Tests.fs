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
open Viewer.Types
open CsQuery

let ParseHtml (resp: string) = CQ.Create(resp)

let MakeRequest httpMethod route =
  runWith defaultConfig (createApp [] )
    |> req httpMethod route None
    |> ParseHtml

let MakeRequestWithVocabs httpMethod route vocabularies =
  runWith defaultConfig (createApp vocabularies)
    |> req httpMethod route None
    |> ParseHtml

[<SetUp>]
let ``Run before tests`` () =
    setTemplatesDir "templates/"

[<Test>]
let ``Visiting the hompage should set the title`` () =
  let title =
    MakeRequest HttpMethod.GET "/"
    |> (fun x -> x.Select("title").Text())
  Assert.AreEqual("KB - Home", title)

[<Test>]
let ``Visiting homepage show heading`` () =
  let header =
    MakeRequest HttpMethod.GET "/"
    |> (fun x -> x.Select("main > h1").Text())
  Assert.AreEqual("NICE Quality Standards", header)

[<Test>]
let ``Visiting homepage should add form with search action`` () =
  let form =
    MakeRequest HttpMethod.GET "/"
    |> (fun x -> x.Select("form"))
  Assert.AreEqual("/search", form.Attr("action"))

[<Test>]
let ``Visiting homepage should present the vocabulary terms`` () =

  let vocabularies = [{Name = "Vocab 1";
                       Terms = [{Name = "Term1"; Uri = "Uri1"};
                                {Name = "Term2"; Uri = "Uri2"}]};
                      {Name = "Vocab 2";
                       Terms = [{Name = "Term3"; Uri = "Uri3"}]}]

  let html = MakeRequestWithVocabs HttpMethod.GET "/" vocabularies

  let vocab1 = html.Select(".vocab").First()
  Assert.True(vocab1.Text().StartsWith("Vocab 1"), "Got: " + vocab1.Text())

  let vocab2 = html.Select(".vocab").Last()
  Assert.True(vocab2.Text().StartsWith("Vocab 2"), "Got: " + vocab2.Text())

  let terms = html.Select(".vocab > input")
  Assert.AreEqual(3, terms.Length)
