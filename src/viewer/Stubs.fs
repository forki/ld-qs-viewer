module Stubs

open FSharp.RDF
open Viewer.Types
open Viewer.ApiTypes
open Viewer.Data.Vocabs.VocabGeneration

let vocabs = [{Root = Term {Uri = (Uri.from "https://nice.org.uk/ontologies/qualitystandard/setting")
                            ShortenedUri = "setting"
                            Label = "Setting"
                            Selected = false
                            Children = [
                                         Term { Uri = Uri.from "https://nice.org.uk/ontologies/core/TestSetting/long-guid-1"
                                                ShortenedUri = "TestSetting/long-guid-1"
                                                Label = "Term1"
                                                Selected = false
                                                Children = [
                                                             Term { Uri = Uri.from "https://nice.org.uk/ontologies/core/TestSetting#long-guid-1A"
                                                                    ShortenedUri = "TestSetting/long-guid-1A"
                                                                    Label = "Term1A"
                                                                    Selected = false
                                                                    Children = [
                                                                                 Term { Uri = Uri.from "https://nice.org.uk/ontologies/core/TestSetting#long-guid-AA"
                                                                                        ShortenedUri = "TestSetting/long-guid-AA"
                                                                                        Label = "Term1 A A"
                                                                                        Selected = false
                                                                                        Children = []};
                                                                   ]};
                                                      Term { Uri = Uri.from "https://nice.org.uk/ontologies/core/TestSetting1#long-guid-1B"
                                                             ShortenedUri = "TestSetting/long-guid-1B"
                                                             Label = "Term1+ B"
                                                             Selected = false
                                                             Children = []};
                                                ]};
                                         Term { Uri = Uri.from "https://nice.org.uk/ontologies/core/TestSetting#long-guid-2"
                                                ShortenedUri = "TestSetting/long-guid-2"
                                                Label = "Term2"
                                                Selected = false
                                                Children = []};
                                       ]};
              Property = "qualitystandard:appliesToSetting";
              Label = "Setting"}
              {Root = Term {Uri = (Uri.from "https://nice.org.uk/ontologies/core/ServiceArea")
                            ShortenedUri = "area"
                            Label = "Service Area"
                            Selected = false
                            Children = [
                                          Term { Uri = Uri.from "https://nice.org.uk/ontologies/core/TestArea#long-guid-3"
                                                 ShortenedUri = "TestArea/long-guid-3"
                                                 Label = "Term3"
                                                 Selected = false
                                                 Children = []};
                                          Term { Uri = Uri.from "https://nice.org.uk/ontologies/core/TestArea#long-guid-4"
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

let private thingycorettl = """@prefix : <https://nice.org.uk/ontologies/core/> .
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

# https://nice.org.uk/ontologies/core/applies_to_thingy
:applies_to_thingy a owl:ObjectProperty ;
	rdfs:subPropertyOf :applies_to ;
	rdfs:label "Thingy"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "applies to thingy"@en .
# 
# https://nice.org.uk/ontologies/core/applies_to

:applies_to a owl:ObjectProperty ;
	rdfs:label "applies to"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "applies to"@en .
# 
"""

let private thingychildttl = """@prefix : <https://nice.org.uk/ontologies/thingy/> .
@prefix owl: <http://www.w3.org/2002/07/owl#> .
@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .
@prefix xml: <http://www.w3.org/XML/1998/namespace> .
@prefix xsd: <http://www.w3.org/2001/XMLSchema#> .
@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#> .

<https://nice.org.uk/ontologies/thingy> a owl:Ontology ;
	owl:imports <http://www.w3.org/2004/02/skos/core> .
# 
# 
# #################################################################
# #
# #    Classes
# #
# #################################################################
# 
# 
# https://nice.org.uk/ontologies/thingy/thingy_class

:thingy_class a owl:Class ;
	rdfs:label "Thingy Class"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "Thingy Class"@en .
# 
# https://nice.org.uk/ontologies/thingy/thingy_level_1
:thingy_level_1 a owl:Class ;
	rdfs:subClassOf :thingy_class ;
	rdfs:label "Thingy Level 1"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "Thingy Level 1 PREF"@en .
# 
# https://nice.org.uk/ontologies/thingy/thingy_level_2

:thingy_level_2 a owl:Class ;
	rdfs:subClassOf :thingy_class ;
	rdfs:label "Thingy Level 2"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "Thingy Level 2 PREF"@en .
# 
# https://nice.org.uk/ontologies/thingy/thingy_level_1_1
:thingy_level_1_1 a owl:Class ;
	rdfs:subClassOf :thingy_level_1 ;
	rdfs:label "Thingy Level 1.1"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "Thingy Level 1.1 PREF"@en .
# 
# https://nice.org.uk/ontologies/thingy/thingy_level_1_2

:thingy_level_1_2 a owl:Class ;
	rdfs:subClassOf :thingy_level_1 ;
	rdfs:label "Thingy Level 1.2"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "Thingy Level 1.2 PREF"@en .
# 
# Generated by the OWL API (version 4.2.5.20160517-0735) https://github.com/owlcs/owlapi
"""

let thingyVocabulary = [
  {
    Root = Term {
      Uri = Uri.from "https://nice.org.uk/ontologies/thingy/thingy_class"
      ShortenedUri = "thingy/thingy_class"
      Label = "Thingy Class"
      Selected = false
      Children = [
        Term {
          Uri = Uri.from "https://nice.org.uk/ontologies/thingy/thingy_level_1"
          ShortenedUri = "thingy/thingy_level_1"
          Label = "Thingy Level 1"
          Selected = false
          Children = [
            Term {
              Uri = Uri.from "https://nice.org.uk/ontologies/thingy/thingy_level_1_1"
              ShortenedUri = "thingy/thingy_level_1_1"
              Label = "Thingy Level 1.1"
              Selected = false
              Children = []
            }
            Term {
              Uri = Uri.from "https://nice.org.uk/ontologies/thingy/thingy_level_1_2"
              ShortenedUri = "thingy/thingy_level_1_2"
              Label = "Thingy Level 1.2"
              Selected = false
              Children = []
            }
          ]
        }
        Term {
          Uri = Uri.from "https://nice.org.uk/ontologies/thingy/thingy_level_2"
          ShortenedUri = "thingy/thingy_level_2"
          Label = "Thingy Level 2"
          Selected = false
          Children = []
        }
      ]
    }
    Property = "core:applies_to_thingy";
    Label = "Thingy"
  }
]

let thingyResponse = {
  contexts = [
    { Prefix = "core"
      Value = "https://nice.org.uk/ontologies/core/" }
    { Prefix = "thingy"
      Value = "https://nice.org.uk/ontologies/thingy/" }
    { Prefix = "whatsit"
      Value = "https://nice.org.uk/ontologies/whatsit/" }
  ]
  properties = [
    { id = "core:applies_to_thingy"
      label = "Thingy";
      options = [
        { id = "thingy:thingy_level_1"
          label = "Thingy Level 1"
          children = [
            { id = "thingy:thingy_level_1_1"
              label = "Thingy Level 1.1"
              children = [] }
            { id = "thingy:thingy_level_1_2"
              label = "Thingy Level 1.2"
              children = [] }
          ]
        }
        { id = "thingy:thingy_level_2"
          label = "Thingy Level 2"
          children = [] }
      ]
    }
  ]
}

let thingyJsonResponse = """{
  "@context": {
    "core": "https://nice.org.uk/ontologies/core/",
    "rdfs": "http://www.w3.org/2000/01/rdf-schema#",
    "thingy": "https://nice.org.uk/ontologies/thingy/",
    "whatsit": "https://nice.org.uk/ontologies/whatsit/"
  },
  "properties": [
    {
      "@id": "core:applies_to_thingy",
      "options": [
        {
          "@id": "thingy:thingy_level_1",
          "children": [
            {
              "@id": "thingy:thingy_level_1_1",
              "rdfs:label": "Thingy Level 1.1"
            },
            {
              "@id": "thingy:thingy_level_1_2",
              "rdfs:label": "Thingy Level 1.2"
            }
          ],
          "rdfs:label": "Thingy Level 1"
        },
        {
          "@id": "thingy:thingy_level_2",
          "rdfs:label": "Thingy Level 2"
        }
      ],
      "rdfs:label": "Thingy"
    }
  ]
}"""

let thingyConfigFile = """{
  "basettluri": "http://schema/ontologies/",
  "baseontologyuri": "https://nice.org.uk/ontologies/",
  "coreontology": {
    "prefix": "core",
    "ontology": "core/",
    "ttl": "core.ttl"
  },
  "childontologies": [ 
    { "prefix": "thingy",
      "ontology": "thingy/",
      "ttl": "thingy.ttl",
      "corereference": "applies_to_thingy"
    },
    { "prefix": "whatsit",
      "ontology": "whatsit/"
    }
  ]
}
"""
let thingyOntologyConfigUri = { CoreTtl= Uri "http://schema/ontologies/core.ttl"
                                Contexts= [ { Prefix="core"; Value= "https://nice.org.uk/ontologies/core/" }
                                            { Prefix="thingy"; Value= "https://nice.org.uk/ontologies/thingy/" }
                                            { Prefix="whatsit"; Value= "https://nice.org.uk/ontologies/whatsit/" } ]
                                Predicates = [
                                  { Uri= "core:applies_to_thingy"; SourceTtl= Uri "http://schema/ontologies/thingy.ttl" }
                                ]
                              }
let thingyOntologyConfigFull = { CoreTtl= Content thingycorettl
                                 Contexts= [ { Prefix="core"; Value= "https://nice.org.uk/ontologies/core/" }
                                             { Prefix="thingy"; Value= "https://nice.org.uk/ontologies/thingy/" }
                                             { Prefix="whatsit"; Value= "https://nice.org.uk/ontologies/whatsit/" } ]
                                 Predicates = [
                                   { Uri= "core:applies_to_thingy"; SourceTtl= Content thingychildttl }
                                 ]
                               }

