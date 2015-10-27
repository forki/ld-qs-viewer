module Viewer.Home

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Types
open Viewer.Utils
open Viewer.VocabGeneration

type HomeModel =  {
   Vocabularies: Vocabulary list
   totalCount: int
}

let home (req:HttpRequest) actualVocabs =
  //printf "Request: %A\n" req

  let testing = req.cookies |> Map.containsKey "test"
  let vocabs = 
    match testing with
    | true  -> Stubs.vocabsForTests
    | false -> actualVocabs

  let filteredVocabs =
    req.query
    |> extractFilters
    |> getVocabsWithState vocabs

  DotLiquid.page "home.html" {Vocabularies = filteredVocabs; totalCount = 0}
