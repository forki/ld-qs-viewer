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

let annotationtool (req:HttpRequest) vocabs =
  let viewVocabs =
    req.query
    |> extractFilters
    |> getVocabsWithState vocabs
    |> List.map (fun v -> {Vocab = v; Expanded = false})

  {Vocabularies = viewVocabs; Yaml = ""}
  |> DotLiquid.page "annotationtool/annotationtool.html"

let toyaml (req:HttpRequest) vocabs =
  let filters = extractFilters req.query
  let viewVocabs =
    filters
    |> getVocabsWithState vocabs
    |> List.map (fun v -> {Vocab = v; Expanded = shouldExpandVocab v.Property filters})

  let yaml = YamlBuilder filters

  {Vocabularies = viewVocabs; Yaml = yaml}
  |> DotLiquid.page "annotationtool/annotationtool.html"

let fromyaml (req:HttpRequest) vocabs =
  let viewVocabs =
    req.query
    |> extractFilters
    |> getVocabsWithState vocabs
    |> List.map (fun v -> {Vocab = v; Expanded = false})

  {Vocabularies = viewVocabs; Yaml = ""}
  |> DotLiquid.page "annotationtool/annotationtool.html"
