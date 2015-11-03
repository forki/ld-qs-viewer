#load "LoadDeps.fsx"

open Swensen.Unquote
open Suave.Testing
open Viewer.Tests.Utils
open CsQuery
open Viewer.VocabGeneration
open FSharp.RDF
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
let output = vocabGeneration graph "Setting"
