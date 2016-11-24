module Stubs
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.RDF


let vocabs = [{Root = Term {Uri = (Uri.from "https://nice.org.uk/ontologies/qualitystandard/setting")
                            ShortenedUri = "setting"
                            Label = "Setting"
                            Selected = false
                            Children = [
                                         Term { Uri = Uri.from "https://nice.org.uk/ontologies/qualitystandard/TestSetting/long-guid-1"
                                                ShortenedUri = "TestSetting/long-guid-1"
                                                Label = "Term1"
                                                Selected = false
                                                Children = [
                                                             Term { Uri = Uri.from "https://nice.org.uk/ontologies/qualitystandard/TestSetting#long-guid-1A"
                                                                    ShortenedUri = "TestSetting/long-guid-1A"
                                                                    Label = "Term1A"
                                                                    Selected = false
                                                                    Children = [
                                                                                 Term { Uri = Uri.from "https://nice.org.uk/ontologies/qualitystandard/TestSetting#long-guid-AA"
                                                                                        ShortenedUri = "TestSetting/long-guid-AA"
                                                                                        Label = "Term1 A A"
                                                                                        Selected = false
                                                                                        Children = []};
                                                                   ]};
                                                      Term { Uri = Uri.from "https://nice.org.uk/ontologies/qualitystandard/TestSetting1#long-guid-1B"
                                                             ShortenedUri = "TestSetting/long-guid-1B"
                                                             Label = "Term1+ B"
                                                             Selected = false
                                                             Children = []};
                                                ]};
                                         Term { Uri = Uri.from "https://nice.org.uk/ontologies/qualitystandard/TestSetting#long-guid-2"
                                                ShortenedUri = "TestSetting/long-guid-2"
                                                Label = "Term2"
                                                Selected = false
                                                Children = []};
                                       ]};
              Property = "qualitystandard:appliesToSetting";
              Label = "Setting"}
              {Root = Term {Uri = (Uri.from "https://nice.org.uk/ontologies/qualitystandard/ServiceArea")
                            ShortenedUri = "area"
                            Label = "Service Area"
                            Selected = false
                            Children = [
                                          Term { Uri = Uri.from "https://nice.org.uk/ontologies/qualitystandard/TestArea#long-guid-3"
                                                 ShortenedUri = "TestArea/long-guid-3"
                                                 Label = "Term3"
                                                 Selected = false
                                                 Children = []};
                                          Term { Uri = Uri.from "https://nice.org.uk/ontologies/qualitystandard/TestArea#long-guid-4"
                                                 ShortenedUri = "TestArea/long-guid-4"
                                                 Label = "Term4"
                                                 Selected = false
                                                 Children = []};]};
              Property = "qualitystandard:appliesToServiceArea";
              Label = "Service Area"}]

let getSearchResults _ _ = [{Uri = "Uri1"; Abstract = "Unicorns under the age of 65..."; Title = "This is the title"; FirstIssued = new System.DateTime()};
                            {Uri = "Uri2"; Abstract = "Goblins with arthritis..."; Title = "Quality standard xxx from quality statement xxx"; FirstIssued = new System.DateTime()};]
                            
let search _ = [{Uri = "Uri1"; Abstract = "Unicorns under the age of 65..."; Title = "This is the title"; FirstIssued = new System.DateTime()};
                {Uri = "Uri2"; Abstract = "Goblins with arthritis..."; Title = "Quality standard xxx from quality statement xxx"; FirstIssued = new System.DateTime()};]

let getKBCount _ = 0

let ontologyConfig = { //TtlRoot= "http://schema/ontologies/"
                       TtlRoot= "http://192.168.99.100/ontologies/"
                       CoreTtl= "qualitystandard.ttl"
                       Contexts= [ { Prefix="qualitystandard"; Value= "https://nice.org.uk/ontologies/qualitystandard/" } ]
                       Predicates = [
                         { Uri= "qualitystandard:62496684_7027_4f37_bd0e_264c9ff727fd"; SourceTtl= "setting.ttl" }
                         { Uri= "qualitystandard:4e7a368e_eae6_411a_8167_97127b490f99"; SourceTtl= "agegroup.ttl" }
                       ]
                     }

let corettl = """@prefix : <https://nice.org.uk/ontologies/qualitystandard/> .
@prefix owl: <http://www.w3.org/2002/07/owl#> .
@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .
@prefix xml: <http://www.w3.org/XML/1998/namespace> .
@prefix xsd: <http://www.w3.org/2001/XMLSchema#> .
@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#> .

<https://nice.org.uk/ontologies/qualitystandard> a owl:Ontology ;
	owl:imports <http://www.w3.org/2004/02/skos/core> .
# 
# 
# #################################################################
# #
# #    Datatypes
# #
# #################################################################
# 
# 
# http://www.w3.org/2001/XMLSchema#date

xsd:date a rdfs:Datatype .
# 
# 
# 
# #################################################################
# #
# #    Object Properties
# #
# #################################################################
# 
# 
# https://nice.org.uk/ontologies/qualitystandard/d828e6c0_40e0_4699_9ec2_d7accf601bc8

:d828e6c0_40e0_4699_9ec2_d7accf601bc8 a owl:ObjectProperty ;
	owl:inverseOf :2a41e426_8990_4c18_a4f3_7f82e42e3d3c ;
	rdfs:label "is replaced by"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "is replaced by"@en .
# 
# https://nice.org.uk/ontologies/qualitystandard/2a41e426_8990_4c18_a4f3_7f82e42e3d3c

:2a41e426_8990_4c18_a4f3_7f82e42e3d3c a owl:ObjectProperty ;
	rdfs:label "replaces"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "replaces"@en .
# 
# https://nice.org.uk/ontologies/qualitystandard/4e7a368e_eae6_411a_8167_97127b490f99

:4e7a368e_eae6_411a_8167_97127b490f99 a owl:ObjectProperty ;
	rdfs:subPropertyOf :693a50d5_304a_4e97_97f3_8f047429ae85 ;
	rdfs:label "Age group"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "applies to age group"@en .
# 
# https://nice.org.uk/ontologies/qualitystandard/62496684_7027_4f37_bd0e_264c9ff727fd

:62496684_7027_4f37_bd0e_264c9ff727fd a owl:ObjectProperty ;
	rdfs:subPropertyOf :693a50d5_304a_4e97_97f3_8f047429ae85 ;
	rdfs:label "Setting"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "applies to setting"@en .
# 
# https://nice.org.uk/ontologies/qualitystandard/693a50d5_304a_4e97_97f3_8f047429ae85

:693a50d5_304a_4e97_97f3_8f047429ae85 a owl:ObjectProperty ;
	rdfs:label "applies to"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "applies to"@en .
# 
"""