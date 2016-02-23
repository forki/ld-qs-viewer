module Viewer.Components.AnnotationBlock

open Suave
open Viewer.Utils
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration

type AnnotationBlockModel = {
  AnnotationBlock : string
  ErrorMessage : string
}

let private serialiseYaml (selected:LabelledFilter list) =
  let createYamlVocabSection acc (vocab, filters) =
    let yamlTerms = filters
                    |> Seq.fold (fun acc filter -> acc + (sprintf "  - \"%s\"\n" (stripAllButFragment filter.TermUri))) ""
    sprintf "%s\n%s" (acc + vocab) yamlTerms

  selected
  |> Seq.groupBy(fun g -> g.VocabLabel)
  |> Seq.fold (fun acc section -> createYamlVocabSection acc section) ""

let private getVocabLabel (filter:Filter) vocabs =
    let v = vocabs |> List.find (fun v -> v.Property = (System.Uri.UnescapeDataString filter.Vocab))
    match v.Root with
        | Empty -> {VocabLabel = ""; TermUri = filter.TermUri}
        | Term t -> {VocabLabel = t.Label + ":"; TermUri = filter.TermUri}

 
let createModel (req:HttpRequest) vocabs convert =
  match convert with
    | true ->
        if (req.rawQuery <> "") then
            let filters = extractFilters req.query
            let yaml =
                filters
                |> List.map (fun f -> getVocabLabel f vocabs)
                |> serialiseYaml

            {AnnotationBlock = yaml; ErrorMessage = ""}
        else
            {AnnotationBlock = ""; ErrorMessage = "Please select an annotation from vocabulary."}

    | false ->
      {AnnotationBlock = ""; ErrorMessage = ""}

