module Viewer.Home

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Types
open Viewer.Utils
open Viewer.VocabGeneration
open FSharp.RDF

let testVocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/setting")
                                Label = "Settings:"
                                Selected = false
                                Children = [
                                             Term { Uri = Uri.from "http://testing.com/TestSetting1"
                                                    Label = "Term1"
                                                    Selected = false
                                                    Children = []};
                                             Term { Uri = Uri.from "http://testing.com/TestSetting2"
                                                    Label = "Term2"
                                                    Selected = false
                                                    Children = []};]};
                   Property = "qualitystandard:setting"}]

type HomeModel =  {
   Vocabularies: Vocabulary list
   totalCount: int
}

let home (req:HttpRequest) actualVocabs =
  //printf "Request: %A\n" req

  let testing = req.cookies |> Map.containsKey "test"
  let vocabs = 
    match testing with
    | true  -> testVocabs
    | false -> actualVocabs

  let filters = extractFilters req.query
  let filteredVocabs = getVocabsWithState vocabs filters
  DotLiquid.page "home.html" {Vocabularies = filteredVocabs; totalCount = 0}
