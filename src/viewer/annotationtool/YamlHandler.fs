module Viewer.YamlHandler

open Viewer.Utils
open Viewer.Types

let YamlBuilder (selected:LabelledFilter list) =

  let createYamlVocabSection acc (vocab, filters) =
    let yamlTerms = filters
                    |> Seq.fold (fun acc filter -> acc + (sprintf "  - %s\n" (stripAllButFragment filter.TermUri))) ""
    sprintf "%s\n%s" (acc + vocab) yamlTerms

  selected
  |> Seq.groupBy(fun g -> g.VocabLabel)
  |> Seq.fold (fun acc section -> createYamlVocabSection acc section) ""
