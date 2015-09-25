module Viewer.Tests.Utils

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open Viewer.App
open Viewer.Types
open CsQuery

type CQ = | CQ of CsQuery.CQ
  with static member select (s:string) (CQ cq)  = cq.Select(s) |> CQ
       static member text (CQ cq) = cq.Text()
       static member attr (s:string) (CQ cq) = cq.Attr s
       static member first (CQ cq) = cq.First() |> CQ
       static member last (CQ cq) = cq.Last() |> CQ
       static member length (CQ cq) = cq.Length

let parseHtml (resp: string) = CQ.Create(resp) |> CQ

let startServerWithData getVocabularies getSearchResults =
  runWith defaultConfig (createApp getVocabularies getSearchResults)

let startServer () =
  let GetVocabularies = []
  let GetSearchResults _ = []

  startServerWithData GetVocabularies GetSearchResults

let get path testCtx = req HttpMethod.GET path None testCtx |> parseHtml
let getQuery path qs testCtx = reqQuery HttpMethod.GET path qs testCtx |> parseHtml


