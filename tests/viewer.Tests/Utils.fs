module Viewer.Tests.Utils

open Suave
open Suave.Web
open Suave.Http
open Suave.Testing
open Suave.Logging
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
  RenderedVocabs = ""
  PerformSearch = (fun _ -> [])
  GetKBCount = (fun _ -> 0)
  HotjarId = ""
  GAId = ""
}

let startServerWith config =
  let suaveConfig = {defaultConfig with logger = Loggers.ConsoleWindowLogger LogLevel.Info}
  runWith suaveConfig (createApp config)
let get path testCtx = reqQuery HttpMethod.GET path "" testCtx |> parseHtml
let getQuery path qs testCtx = reqQuery HttpMethod.GET path qs testCtx |> parseHtml

let uri (s:string) =
  Uri.from s

let t = {Label = ""
         ShortenedUri = ""
         Uri = uri "http://somewhere.com"
         Selected = false;
         Children = []}

