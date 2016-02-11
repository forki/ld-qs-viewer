module Viewer.Tests.Utils

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open Viewer.App
open Viewer.AppConfig
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.RDF
open CsQuery

type CQ = | CQ of CsQuery.CQ
  with static member select (s:string) (CQ cq)  = cq.Select(s) |> CQ
       static member text (CQ cq) = cq.Text()
       static member attr (s:string) (CQ cq) = cq.Attr s
       static member first (CQ cq) = cq.First() |> CQ
       static member last (CQ cq) = cq.Last() |> CQ
       static member length (CQ cq) = cq.Length
       static member cq (CQ cq) = cq

let parseHtml (resp: string) = CQ.Create(resp) |> CQ

let baseConfig = {
  Vocabs = []
  GetSearchResults = (fun _ _ -> [])
  GetKBCount = (fun _ -> 0)
  HotjarId = ""
}

let startServerWith config =
  runWith defaultConfig (createApp config)
let get path testCtx = reqQuery HttpMethod.GET path "" testCtx |> parseHtml
let getQuery path qs testCtx = reqQuery HttpMethod.GET path qs testCtx |> parseHtml

let uri (s:string) =
  Uri.from s

let t = {Label = ""
         Uri = uri "http://somewhere.com"
         Selected = false;
         Children = []}

