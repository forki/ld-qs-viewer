module Viewer.App

open Suave
open Suave.Web
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Files
open Suave.Http.Successful
open Suave.Types
open Suave.Log
open Suave.Utils
open Viewer.Types
open Viewer.Elastic
open FSharp.Data

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

type HomeModel =  {
   Vocabularies: Vocabulary list
 }

type SearchModel = {
  Results: SearchResult list

  }

let escapeCharsInString str =
  let chars = ["+";"-";"=";"&";"|";">";"<";"!";"(";")";"{";"}";"[";"]";"^";"\"";"~";"*";"?";":";"/"]
  let esc = "\\"
  Seq.fold (fun (acc:string) elem -> acc.Replace(elem, esc+elem)) str chars

let buildQuery qs =
  qs |> Seq.head
  |> (fun a ->
      match a with
      | (k, Some(v)) -> (sprintf "q=%s:%s" (escapeCharsInString k) (escapeCharsInString v)))

let createApp vocabularies getSearchResultsFor =
  choose
    [ GET >>= choose
        [path "/" >>= DotLiquid.page "home.html" {Vocabularies = vocabularies}
         path "/search" >>= request(fun r ->
                                    let query = buildQuery(r.query)
                                    DotLiquid.page "search.html" {Results = (getSearchResultsFor query)})

         RequestErrors.NOT_FOUND "Found no handlers"]]


