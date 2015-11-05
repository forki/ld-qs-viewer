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
open Viewer.VocabGeneration
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
}

let startServerWith config =
  runWith defaultConfig (createApp config)

let get path testCtx = req HttpMethod.GET path None testCtx |> parseHtml
let getQuery path qs testCtx = reqQuery HttpMethod.GET path qs testCtx |> parseHtml

type VocabularyBuilder () =
  member x.childDefault =
    { Uri = Uri.from "http://test.com/uri#uri1"
      Label = "Term1"
      Selected = false
      Children = []
      }

  member x.RootDefault =
    { Root = Term { Uri = Uri.from "http://test.com/uri#uri1";
                     Label = "Term1";
                     Selected = false;
                     Children = [];};
       Property = "Property 1"
       }

  member x.AddChildrenToRoot parent children =
    match parent with
      | { Root = root; Property = property } ->
        match root with
          | Term y ->
            match y with
              | { Uri = uri;Label = lbl; Selected = selected; Children = xsChildren;} ->
                {Root = Term { Uri = uri; Label = lbl; Selected = selected;
                        Children = (children @ xsChildren)};
                 Property = property}

  member x.createRoot ((uri:string), lbl, selected, children, property) =
    [{ Root = Term { Uri = Uri.from uri; Label = lbl; Selected = selected; Children = children}; Property = property}]

  member x.createChild ((uri:string), lbl, selected, children) =
    Term { Uri = Uri.from uri; Label = lbl; Selected = selected; Children = children}
