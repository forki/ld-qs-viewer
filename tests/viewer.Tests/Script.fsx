#load "LoadDeps.fsx"

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open CsQuery
open Viewer.Tests
open Viewer.Tests.Utils
open Viewer.App
open Viewer.Types
open Suave.DotLiquid
open FSharp.Data
open Swensen.Unquote

//let html = CQ ("""
//               <div class="grandparent">
//                    <div class="parent">
//                        <div>First</div>
//                        <div>Second</div>
//                    </div
//               </div
//               """)
//
//let divs = html.Select("")
setTemplatesDir "templates/"

let test1 () =
  let GetSearchResults _ = [{Uri = "Result1"; Abstract = "Abstract1"};
                            {Uri = "Result2"; Abstract = "Abstract2"}]
  let GetVocabularies = []

  let dom =
    startServerWithData GetVocabularies GetSearchResults
    |> reqQuery HttpMethod.GET "/search" "q=1" 
    |> ParseHtml

  let abstracts = dom.Select(".result > .abstract")

  test <@ abstracts.First().Text() = "Abstract1" @>
  test <@ abstracts.Last().Text() = "Abstract2" @>
