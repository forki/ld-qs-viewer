module Viewer.Home

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Model
open Viewer.Utils
open Viewer.VocabGeneration
open Viewer.Elastic

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

  let KBDocTotal =
    match testing with
      | true -> 0
      | false -> Elastic.KnowledgeBaseCount testing

  {Results = []
   Tags = []
   Vocabularies = viewVocabs
   totalCount = KBDocTotal
   ShowHelp = true}
  |> DotLiquid.page "home.html"
