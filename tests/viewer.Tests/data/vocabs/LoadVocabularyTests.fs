module Viewer.Tests.Data.Vocabs.LoadVocabularyTests

open NUnit.Framework
open FsUnit
open FSharp.RDF
open Viewer.Data.Vocabs.VocabGeneration
open Viewer.Types
open Viewer.Tests.Utils

let graph = """@base <https://nice.org.uk/ontologies/setting>.

@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>.
@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#>.
@prefix xsd: <http://www.w3.org/2001/XMLSchema#>.
@prefix base: <https://nice.org.uk/ontologies/setting>.
@prefix owl: <http://www.w3.org/2002/07/owl#>.

<https://nice.org.uk/ontologies/setting#Primary%20care%20setting> a owl:Class,
                                                                              owl:NamedIndividual;
                                                                            rdfs:label "Primary care setting"^^xsd:string;
                                                                            rdfs:subClassOf <https://nice.org.uk/ontologies/setting#Setting>.
<https://nice.org.uk/ontologies/setting#Hospital> a owl:Class,
                                                              owl:NamedIndividual;
                                                            rdfs:label "Hospital"^^xsd:string;
                                                            rdfs:subClassOf <https://nice.org.uk/ontologies/setting#Primary%20care%20setting>.

<https://nice.org.uk/ontologies/setting#Community> a owl:Class,
                                                               owl:NamedIndividual;
                                                             rdfs:label "Community"^^xsd:string;
                                                             rdfs:subClassOf <https://nice.org.uk/ontologies/setting#Primary%20care%20setting>.
  """

[<Test>]
let ``When parsing graph will return sorted tree`` () =
  let output = vocabGeneration graph "Setting"
  output
  |> should equal ( Term {Uri = Uri.from "https://nice.org.uk/ontologies/setting#Primary%20care%20setting"
                          ShortenedUri = "setting#Primary care setting"
                          Label = "Primary care setting"
                          Selected = false
                          Children = [
                                       Term {Uri = Uri.from "https://nice.org.uk/ontologies/setting#Community"
                                             ShortenedUri = "setting#Community"
                                             Label = "Community"
                                             Selected = false
                                             Children = []}
                                       Term {Uri = Uri.from "https://nice.org.uk/ontologies/setting#Hospital"
                                             ShortenedUri = "setting#Hospital"
                                             Label = "Hospital"
                                             Selected = false
                                             Children = []}]})

    
[<Test>]
let ``Should set vocab term as selected if url exists in filters`` () =
  let vocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                              ShortenedUri = "setting"
                              Label = "Vocab 1"
                              Selected = false
                              Children = [
                                           Term { Uri = Uri.from "http://testing.com/Uri1"
                                                  ShortenedUri = "setting"
                                                  Label = "Term1"
                                                  Selected = false
                                                  Children = []};
                                           Term { Uri = Uri.from "http://testing.com/Uri2"
                                                  ShortenedUri = "setting"
                                                  Label = "Term2"
                                                  Selected = false
                                                  Children = []};
                                           Term { Uri = Uri.from "http://testing.com/Uri3"
                                                  ShortenedUri = "setting"
                                                  Label = "Term3"
                                                  Selected = false
                                                  Children = []}]};
                 Property = "v1";
                 Label = ""}]

  let expectedVocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                                      ShortenedUri = "setting"
                                      Label = "Vocab 1"
                                      Selected = false
                                      Children = [
                                                   Term { Uri = Uri.from "http://testing.com/Uri1"
                                                          ShortenedUri = "setting"
                                                          Label = "Term1"
                                                          Selected = false
                                                          Children = []};
                                                   Term { Uri = Uri.from "http://testing.com/Uri2"
                                                          ShortenedUri = "setting"
                                                          Label = "Term2"
                                                          Selected = true
                                                          Children = []};
                                                   Term { Uri = Uri.from "http://testing.com/Uri3"
                                                          ShortenedUri = "setting"
                                                          Label = "Term3"
                                                          Selected = true
                                                          Children = []}]};
                         Property = "v1";
                         Label = ""}]

  let filters = [{Vocab = "notused"; TermUri = "http://testing.com/Uri2"}
                 {Vocab = "notused"; TermUri = "http://testing.com/Uri3"}]
  let actualVocabs = getVocabsWithState vocabs filters 
  actualVocabs |> should equal expectedVocabs


[<Test>]
let ``Should have unselected checkboxes when no search term in url`` () =
  let vocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                              ShortenedUri = "setting"
                              Label = "Vocab 1"
                              Selected = false
                              Children = [
                                           Term { Uri = Uri.from "http://testing.com/Uri1"
                                                  ShortenedUri = "setting"
                                                  Label = "Term1"
                                                  Selected = false
                                                  Children = []};
                                           Term { Uri = Uri.from "http://testing.com/Uri2"
                                                  ShortenedUri = "setting"
                                                  Label = "Term2"
                                                  Selected = false
                                                  Children = []};
                                           Term { Uri = Uri.from "http://testing.com/Uri3"
                                                  ShortenedUri = "setting"
                                                  Label = "Term3"
                                                  Selected = false
                                                  Children = []}]};
                 Property = "v1";
                Label = ""}]

  let expectedVocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                                      ShortenedUri = "setting"
                                      Label = "Vocab 1"
                                      Selected = false
                                      Children = [
                                                   Term { Uri = Uri.from "http://testing.com/Uri1"
                                                          ShortenedUri = "setting"
                                                          Label = "Term1"
                                                          Selected = false
                                                          Children = []};
                                                   Term { Uri = Uri.from "http://testing.com/Uri2"
                                                          ShortenedUri = "setting"
                                                          Label = "Term2"
                                                          Selected = true
                                                          Children = []};
                                                   Term { Uri = Uri.from "http://testing.com/Uri3"
                                                          ShortenedUri = "setting"
                                                          Label = "Term3"
                                                          Selected = true
                                                          Children = []}]};
                         Property = "v1";
                         Label = ""}]

  let filters = [{Vocab = ""; TermUri = "http://testing.com/Uri2"}
                 {Vocab = ""; TermUri = "http://testing.com/Uri3"}]
  let actualVocabs = getVocabsWithState vocabs filters 
  actualVocabs |> should equal expectedVocabs


[<Test>]
let ``Should present a vocabulary with a single term as an input checkbox`` () =
  let vocabs = [{Property = "vocab"
                 Root = Term {t with Label = "Vocab 1"
                                     Children = [Term {t with Label = "Term1"; Uri = uri "http://testing.com/Uri1"; ShortenedUri="Uri1"}]};
                 Label = "Vocab 1"}]

  let html = renderVocabs vocabs |> parseHtml

  html
  |> CQ.select ".vocab"
  |> CQ.first
  |> CQ.text
  |> should contain "Vocab 1"

  let checkboxes =
    html
    |> CQ.select "input[type='checkbox']"
    |> CQ.select ".term"

  checkboxes
  |> CQ.first
  |> CQ.attr "value"
  |> should equal "Uri1"

  checkboxes
  |> CQ.first
  |> CQ.attr "name"
  |> should equal "vocab"

  html
  |> CQ.select ".checkbox > label"
  |> CQ.first
  |> CQ.text
  |> should equal "Term1"
    
[<Test>]
let ``Should present the multiple vocabulary containing multiple terms`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t
                                                 Term t]};
                 Label = ""}]

  renderVocabs vocabs
  |> parseHtml
  |> CQ.select "input[type='checkbox']"
  |> CQ.select ".term"
  |> CQ.length
  |> should equal 2
    
[<Test>]
let ``Should present the vocabulary term checkboxes unselected by default`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = [Term t]};
                 Label = ""}]

  renderVocabs vocabs
  |> parseHtml
  |> CQ.select "input[checked]"
  |> CQ.length
  |> should equal 0

[<Test>]
let ``Should present the vocabulary collapsed by default`` () =
  let vocabs = [{Property = ""
                 Root = Term {t with Children = []}
                 Label = ""}]

  renderVocabs vocabs
  |> parseHtml
  |> CQ.select ".accordion.closed"
  |> CQ.length
  |> should equal 1

[<Test>]
let ``Should render the vocabs with vocabulary property embedded in dom`` () =
  let vocabs = [{Property = "vocab:property"
                 Root = Term t
                 Label = ""}]

  renderVocabs vocabs
  |> parseHtml
  |> CQ.select ".accordion-trigger"
  |> CQ.attr "id"
  |> should equal "vocab:property"
