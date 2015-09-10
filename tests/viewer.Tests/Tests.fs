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

let ParseHtml (resp: string) = CQ.Create(resp)

let MakeRequest httpMethod route =
  runWith defaultConfig (createApp [("","")])
    |> req httpMethod route None
    |> ParseHtml

let MakeRequest' httpMethod route fAnnotations =
  runWith defaultConfig (createApp fAnnotations)
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
let ``Visiting homepage should present annotations`` () =
  let getAnnotations =
    [("Annotation Name", "http://ld.nice.org.uk/ns/Annotation_Uri")]

  let checkbox =
    MakeRequest' HttpMethod.GET "/" getAnnotations
    |> (fun x -> x.Select("input"))
  Assert.AreEqual("Annotation Name", checkbox.Attr("name"))
  Assert.AreEqual("http://ld.nice.org.uk/ns/Annotation_Uri", checkbox.Attr("value"))

[<Test>]
let ``Visiting homepage should present a list of annotations`` () =
  let getAnnotations =
    [("Annotation 1", "http://ld.nice.org.uk/ns/Annotation_Uri1");
     ("Annotation 2", "http://ld.nice.org.uk/ns/Annotation_Uri2")]
  
  let checkboxes =
    MakeRequest' HttpMethod.GET "/" getAnnotations
    |> (fun x -> x.Select("input"))
  
  Assert.AreEqual(2, checkboxes.Length)
