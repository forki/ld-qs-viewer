module Viewer.Home

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Model
open Viewer.Utils
open Viewer.VocabGeneration

let home (req:HttpRequest) actualVocabs =
  //printf "Request: %A\n" req

  let testing = req.cookies |> Map.containsKey "test"
  let vocabs = 
    match testing with
    | true  -> Stubs.vocabsForTests
    | false -> actualVocabs

  let viewVocabs =
    req.query
    |> extractFilters
    |> getVocabsWithState vocabs
    |> List.map (fun v -> {Vocab = v; Expanded = false})

  {Results = []
   Tags = []
   Vocabularies = viewVocabs
   totalCount = 0
   ShowHelp = true}
  |> DotLiquid.page "home.html"
