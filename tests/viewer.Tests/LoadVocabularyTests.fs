module Viewer.Tests.LoadVocabularyTests

open Swensen.Unquote
open NUnit.Framework
open FSharp.RDF
open Viewer.VocabGeneration
open Viewer.Types

let graph = """@base <http://ld.nice.org.uk/ns/qualitystandard/setting>.

@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>.
@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#>.
@prefix xsd: <http://www.w3.org/2001/XMLSchema#>.
@prefix base: <http://ld.nice.org.uk/ns/qualitystandard/setting>.
@prefix owl: <http://www.w3.org/2002/07/owl#>.

<http://ld.nice.org.uk/ns/qualitystandard/setting#Primary%20care%20setting> a owl:Class,
                                                                              owl:NamedIndividual;
                                                                            rdfs:label "Primary care setting"^^xsd:string;
                                                                            rdfs:subClassOf <http://ld.nice.org.uk/ns/qualitystandard/setting#Setting>.
<http://ld.nice.org.uk/ns/qualitystandard/setting#Hospital> a owl:Class,
                                                              owl:NamedIndividual;
                                                            rdfs:label "Hospital"^^xsd:string;
                                                            rdfs:subClassOf <http://ld.nice.org.uk/ns/qualitystandard/setting#Primary%20care%20setting>.

<http://ld.nice.org.uk/ns/qualitystandard/setting#Community> a owl:Class,
                                                               owl:NamedIndividual;
                                                             rdfs:label "Community"^^xsd:string;
                                                             rdfs:subClassOf <http://ld.nice.org.uk/ns/qualitystandard/setting#Primary%20care%20setting>.
  """


[<Test>]
let ``Parsing graph will return sorted tree`` () =
  let output = vocabGeneration graph "Setting"
  test <@ output =
             Term {Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/setting#Primary%20care%20setting"
                   Label = "Primary care setting"
                   Selected = false
                   Children = [
                       Term {Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/setting#Community"
                             Label = "Community"
                             Selected = false
                             Children = []}
                       Term {Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/setting#Hospital"
                             Label = "Hospital"
                             Selected = false
                             Children = []}

             ]}
    @>



[<Test>]
let ``Should set vocab term as selected if url exists in filters`` () =
  let vocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                              Label = "Vocab 1"
                              Selected = false
                              Children = [
                                           Term { Uri = Uri.from "http://testing.com/Uri1"
                                                  Label = "Term1"
                                                  Selected = false
                                                  Children = []};
                                           Term { Uri = Uri.from "http://testing.com/Uri2"
                                                  Label = "Term2"
                                                  Selected = false
                                                  Children = []};
                                           Term { Uri = Uri.from "http://testing.com/Uri3"
                                                  Label = "Term3"
                                                  Selected = false
                                                  Children = []}]};
                 Property = "v1"}]

  let expectedVocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                                      Label = "Vocab 1"
                                      Selected = false
                                      Children = [
                                                   Term { Uri = Uri.from "http://testing.com/Uri1"
                                                          Label = "Term1"
                                                          Selected = false
                                                          Children = []};
                                                   Term { Uri = Uri.from "http://testing.com/Uri2"
                                                          Label = "Term2"
                                                          Selected = true
                                                          Children = []};
                                                   Term { Uri = Uri.from "http://testing.com/Uri3"
                                                          Label = "Term3"
                                                          Selected = true
                                                          Children = []}]};
                         Property = "v1"}]

  let filters = [{Vocab = "notused"; TermUri = "http://testing.com/Uri2"}
                 {Vocab = "notused"; TermUri = "http://testing.com/Uri3"}]
  let actualVocabs = getVocabsWithState vocabs filters 
  test <@ actualVocabs = expectedVocabs @>
