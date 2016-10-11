module Viewer.Tests.Components.AnnotationBlock.Tests

open Suave
open NUnit.Framework
open FsUnit
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Tests.Utils
open Viewer.SuaveExtensions
open FSharp.RDF

[<SetUp>]
let ``Run before tests`` () =
  setTemplatesDir "../../../../bin/viewer/web/qs/"

let vocabs = [{Property = "vocab:property";
               Root = Term {t with Label = "Vocab Label";
                                   Children = [Term {t with Uri = uri "http://testing.com/Uri#Term1"}
                                               Term {t with Uri = uri "http://testing.com/Uri#Term2"}]}
               Label = ""}]
    

[<Test>]
let ``Should generate annotations and human readable annotation block from querystring`` () =
  let vocabs = [{Property = "vocab:property"
                 Root = Term {t with 
                                  ShortenedUri = "some/thing"
                                  Label = "Vocab Label";
                                  Children = [Term {t with 
                                                      ShortenedUri = "vocabLabel/long-guid-1"
                                                      Label = "Term1"
                                                      Uri = uri "http://testing.com/vocabLabel/long-guid-1"};
                                             Term {t with 
                                                      ShortenedUri = "vocabLabel/long-guid-2"
                                                      Label = "Term2"
                                                      Uri = uri "http://testing.com/vocabLabel/long-guid-2"}]}
                 Label = ""}]

  let qsWithTwoVocabTerms = "vocab%3Aproperty=vocabLabel%2Flong-guid-1&vocab%3Aproperty=vocabLabel%2Flong-guid-2"

  let yaml = startServerWith {baseConfig with Vocabs = vocabs}
             |> getQuery "/annotationtool/toyaml" qsWithTwoVocabTerms
  let annotations =
       yaml
       |> CQ.select "#annotations"
       |> CQ.text
      
  let human_readable =
       yaml
       |> CQ.select "#human-readable-annotations"
       |> CQ.text

  human_readable |> should equal "Vocab Label:\n  - \"Term1\"\n  - \"Term2\"\n"
  annotations |> should equal "Vocab Label:\n  - \"long-guid-1\"\n  - \"long-guid-2\"\n"
    
[<Test>]
let ``Should produce error upon no vocabulary selection`` () =
  let errorMessage = startServerWith {baseConfig with Vocabs = vocabs}
                     |> getQuery "/annotationtool/toyaml" ""
                     |> CQ.select ".message"
                     |> CQ.text

  errorMessage |> should equal "Please select an annotation from vocabulary."
