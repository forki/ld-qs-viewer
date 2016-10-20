module Viewer.Components.AnnotationBlock

open Suave
open Viewer.Utils
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.Data

type AnnotationBlockModel = {
  AnnotationBlock : string
  HumanReadable : string
  ErrorMessage : string
}


let private serialiseYaml (selected:LabelledFilter list) =
  let createYamlVocabSection acc (vocab, filters) =
    let yamlTerms = filters
                    |> Seq.fold (fun acc filter -> acc + (sprintf "  - \"%s\"\n" (filter.TermUri))) ""
    sprintf "%s\n%s" (acc + vocab) yamlTerms

  selected
  |> Seq.groupBy(fun g -> g.VocabLabel)
  |> Seq.fold (fun acc section -> createYamlVocabSection acc section) ""

let private getVocabLabel (filter:Filter) vocabs getTermUri =
  let v = vocabs |> List.tryFind (fun v -> v.Property = (System.Uri.UnescapeDataString filter.Vocab))
  match v with
  | None -> {VocabLabel = "NO VOCABULARY FOUND"; TermUri = "PARENT NOT FOUND"}
  | Some v -> match v.Root with
              | Empty -> {VocabLabel = ""; TermUri = filter.TermUri}
              | Term t -> {VocabLabel = t.Label + ":"; TermUri = getTermUri}

let createModel (req:HttpRequest) vocabs convert =
  let flatVocab = flattenVocab vocabs
  match convert with
    | true ->
        if (req.rawQuery <> "") then
            let filters = extractFilters req.query

            let yaml =
                filters
                |> List.map (fun f -> getVocabLabel f vocabs (getGuidFromFilter f))
                |> serialiseYaml

            let humanReadableYaml =
                filters
                |> List.map (fun f -> getVocabLabel f vocabs (getLabelFromGuid flatVocab f))
                |> serialiseYaml
            
            {AnnotationBlock = yaml; HumanReadable=humanReadableYaml; ErrorMessage = ""}
        else
            {AnnotationBlock = ""; HumanReadable=""; ErrorMessage = "Please select an annotation from vocabulary."}
    | false ->
      {AnnotationBlock = ""; HumanReadable=""; ErrorMessage = ""}

