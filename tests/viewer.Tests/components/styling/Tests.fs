module Viewer.Tests.Components.Styling.Tests

open Suave
open NUnit.Framework
open FsUnit
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.SuaveExtensions
open Suave.Testing
open FSharp.RDF

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "../../../../bin/viewer/web/qs/"

let vocabs = [{Property = "vocab:property";
               Root = Term {t with Label = "Vocab Label";
                                   Children = [Term {t with Uri = uri "http://testing.com/Uri#Term1"}
                                               Term {t with Uri = uri "http://testing.com/Uri#Term2"}]}
               Label = ""}]

//let runServer () =
//  let defaultConfig = {config with bindings = [ HttpBinding.mkSimple HTTP "127.0.0.1" 8083 ]}
//  runWith defaultConfig app
//
//let respParse (response : HttpResponseMessage) =
//  {StatusCode = response.StatusCode
//   Content = response.Content.ReadAsStringAsync().Result}
//
//let coreReq methd path data =
//  reqResp methd path "" data None System.Net.DecompressionMethods.None id respParse
//
//let post path content =
//  use data = new StringContent(content)
//  runServer() |> coreReq HttpMethod.POST path (Some data)
//
//let get path =
//  runServer() |> coreReq HttpMethod.GET path None
// 
 //let getQuery path qs testCtx = reqQuery HttpMethod.GET path qs testCtx |> parseHtml
   
[<Test>]
let ``Styling: Should find the discovery tool style sheet`` () =
  let responseMessage = startServerWith {baseConfig with Vocabs = vocabs}
//                        |> getQuery "/annotationtool/toyaml" ""
//                        |> CQ.select ".message"
//                        |> CQ.text
                        |> get "/qs"
             
  responseMessage |> should equal "Hello Dave"
