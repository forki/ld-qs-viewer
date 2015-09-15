module Viewer.Tests.Utils

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open Viewer.App
open CsQuery

let ParseHtml (resp: string) = CQ.Create(resp)

let NoSearchResults () = """{"hits":{"hits":[]}}"""

let startServerWithData getVocabularies getSearchResults =
  runWith defaultConfig (createApp getVocabularies getSearchResults)

let startServer () =
  let GetVocabularies = []
  startServerWithData GetVocabularies NoSearchResults

