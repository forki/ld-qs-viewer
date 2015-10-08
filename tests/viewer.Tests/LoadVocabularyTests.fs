module Viewer.Tests.LoadVocabularyTests

open Swensen.Unquote
open NUnit.Framework
open Viewer.VocabGeneration
open Viewer.Types

let singleTierGraph = """@base <http://ld.nice.org.uk/ns/qualitystandard/setting>.

@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>.
@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#>.
@prefix xsd: <http://www.w3.org/2001/XMLSchema#>.
@prefix base: <http://ld.nice.org.uk/ns/qualitystandard/setting>.
@prefix owl: <http://www.w3.org/2002/07/owl#>.

<http://ld.nice.org.uk/ns/qualitystandard/setting#Primary%20care%20setting> a owl:Class,
                                                                              owl:NamedIndividual;
                                                                            rdfs:label "Primary care setting"^^xsd:string;
                                                                            rdfs:subClassOf <http://ld.nice.org.uk/ns/qualitystandard/setting#Setting>.
<http://ld.nice.org.uk/ns/qualitystandard/setting#Community> a owl:Class,
                                                               owl:NamedIndividual;
                                                             rdfs:label "Community"^^xsd:string;
                                                             rdfs:subClassOf <http://ld.nice.org.uk/ns/qualitystandard/setting#Setting>.
  """
(*
[<Test>]
let ``Parse flat graph will return a flat list`` () =
  let output = vocabGeneration singleTierGraph
  test <@ output = [
            { Name = "Primary care setting"; Uri = "http://ld.nice.org.uk/ns/qualitystandard/setting#Primary care setting"}
            { Name = "Community"; Uri = "http://ld.nice.org.uk/ns/qualitystandard/setting#Community"}
    ] @>
*)
