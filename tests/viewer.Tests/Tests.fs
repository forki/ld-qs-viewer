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
  runWith defaultConfig (createApp {Vocabularies = []} )
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

//[<Test>]
//let ``Visiting homepage should present annotations`` () =
//  let getAnnotations =
//    [("Annotation Name", "http://ld.nice.org.uk/ns/Annotation_Uri")]
//
//  let checkbox =
//    MakeRequest' HttpMethod.GET "/" getAnnotations
//    |> (fun x -> x.Select("input"))
//  Assert.AreEqual("Annotation Name", checkbox.Attr("name"))
//  Assert.AreEqual("http://ld.nice.org.uk/ns/Annotation_Uri", checkbox.Attr("value"))


[<Test>]
let ``Visiting homepage should present the vocabulary terms`` () =

  let vocabularies = {Vocabularies = [{Name = "Vocab1";
                                       Terms = [{Name = "Term1"; Uri = "Uri1"};
                                                {Name = "Term2"; Uri = "Uri2"}]};
                                      {Name = "Vocab2";
                                       Terms = [{Name = "Term3"; Uri = "Uri3"}]}]}

  let html = MakeRequestWithVocabs HttpMethod.GET "/" vocabularies

  let vocab1 =
    html
    |> (fun x -> x.Select("#Vocab1"))

  Assert.AreEqual("Vocab1", vocab1.Text())

  let vocab1inputs = 
    vocab1 
    |> (fun x -> x.Select("#Vocab1 > input"))

  Assert.AreEqual(2, vocab1inputs.Length)

  let vocab2 = 
    html
    |> (fun x -> x.Select("#Vocab2"))

  Assert.AreEqual("Vocab2", vocab2.Text())

  let vocab2inputs = 
    vocab2
    |> (fun x -> x.Select("#Vocab2 > input"))

  Assert.AreEqual(1, vocab1inputs.Length)
