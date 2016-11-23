module Viewer.Tests.Data.Annotations.AnnotationEndpointTests

open NUnit.Framework
open Suave
open FsUnit
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.SuaveExtensions
open FSharp.RDF
open Viewer.YamlParser
open Viewer.Utils
open FsUnit

[<Test>]
let ``toguidblock endpoint should convert querystring and return yaml with guids`` () =
  let vocabs = [{Property = "vocab:property"
                 Root = Term {t with 
                                  Children = [Term {t with ShortenedUri = "vocab/long-guid-1"}
                                              Term {t with ShortenedUri = "vocab/long-guid-2"}]}
                 Label = ""}]

  let qsWithTwoVocabTerms = "vocab%3Aproperty=vocab%2Flong-guid-1&vocab%3Aproperty=vocab%2Flong-guid-2"

  let yaml = startServerWith {baseConfig with Vocabs = vocabs}
             |> getQ "/annotationtool/toguidblock" qsWithTwoVocabTerms
      
  yaml |> should equal "vocab:property:\n  - \"vocab/long-guid-1\"\n  - \"vocab/long-guid-2\"\n"

(*
[<Test>]
let ``tolabelblock endpoint should convert querystring and return yaml with labels`` () =
    let vocabs = [{Property = "vocab:property"
                   Root = Term {t with 
                                    Label = "Vocab"
                                    Children = [Term {t with ShortenedUri = "vocab/long-guid-1"; Label = "Term1"}
                                                Term {t with ShortenedUri = "vocab/long-guid-2"; Label = "Term2"}]}
                   Label = ""}]

    let qsWithTwoVocabTerms = "vocab%3Aproperty=vocab%2Flong-guid-1&vocab%3Aproperty=vocab%2Flong-guid-2"

    let yaml = startServerWith {baseConfig with Vocabs = vocabs}
        |> getQ "/annotationtool/toguidblock" qsWithTwoVocabTerms

  yaml |> should equal "vocab:property:\n  - \"vocab/long-guid-1\"\n  - \"vocab/long-guid-2\"\n"
*)
