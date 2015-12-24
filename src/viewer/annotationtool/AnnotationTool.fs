module Viewer.AnnotationTool

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Types
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

  let getVocabLabel (filter:Filter) =
    let v = vocabs |> List.find (fun v -> v.Property = (System.Uri.UnescapeDataString filter.Vocab))
    match v.Root with
      | Empty -> {VocabLabel = ""; TermUri = filter.TermUri}
      | Term t -> {VocabLabel = t.Label; TermUri = filter.TermUri}
    
  let filters = extractFilters req.query
  let viewVocabs =
    filters
    |> getVocabsWithState vocabs
    |> List.map (fun v -> {Vocab = v; Expanded = shouldExpandVocab v.Property filters})

  let yaml = filters
             |> List.map (fun f -> getVocabLabel f)
             |> YamlBuilder

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
