module Viewer.AnnotationTool

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Model
open Viewer.Utils
open Viewer.VocabGeneration
open Viewer.Elastic

type Annotation = {
    Vocabularies : ViewVocab list
    }
let annotationtool (req:HttpRequest) actualVocabs =
//    {Vocabularies = viewVocabs}
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

    {Vocabularies = viewVocabs}
    |> DotLiquid.page "annotationtool/annotationtool.html"
