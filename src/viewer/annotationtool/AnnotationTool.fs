module Viewer.AnnotationTool

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Model
open Viewer.Utils
open Viewer.VocabGeneration
open Viewer.Elastic
open Viewer.YamlHandler

type Annotation = {
    Vocabularies : ViewVocab list
    Yaml : string
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

    {Vocabularies = viewVocabs; Yaml = ""}
    |> DotLiquid.page "annotationtool/annotationtool.html"



let toyaml (req:HttpRequest) actualVocabs =

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

    let qs = req.query
    let filters = extractFilters qs
    let yaml = YamlBuilder filters

    {Vocabularies = viewVocabs; Yaml = yaml}
    |> DotLiquid.page "annotationtool/annotationtool.html"



let fromyaml (req:HttpRequest) actualVocabs =

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

    {Vocabularies = viewVocabs; Yaml = ""}
    |> DotLiquid.page "annotationtool/annotationtool.html"

