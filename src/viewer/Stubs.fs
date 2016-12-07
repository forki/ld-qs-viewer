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

let private dummyCoreTtl = """@prefix : <https://nice.org.uk/ontologies/core/> .
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
	rdfs:label "Applies to thingy"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "applies to thingy"@en .
# 
# https://nice.org.uk/ontologies/core/applies_to

:applies_to a owl:ObjectProperty ;
	rdfs:label "applies to"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "applies to"@en .
# 
# https://nice.org.uk/ontologies/core/GUID_stringProperty

:GUID_stringProperty a owl:ObjectProperty ;
	rdfs:label "This thingy"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "has this thingy"@en .
#
# 
# 
# 
# #################################################################
# #
# #    Data properties
# #
# #################################################################
# 
# 
# https://nice.org.uk/ontologies/core/GUID_conditionalProperty

:GUID_conditionalProperty a owl:DatatypeProperty ;
	rdfs:range xsd:date ;
	rdfs:label "That thingy changed"@en ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "has that thingy changed on"@en .
# 

# https://nice.org.uk/ontologies/qualitystandard/GUID_boolProperty

:GUID_boolProperty a owl:DatatypeProperty ;
	rdfs:range xsd:boolean ;
	<http://www.w3.org/2004/02/skos/core#prefLabel> "is that thingy"@en .
# 
# Generated by the OWL API (version 4.2.5.20160517-0735) https://github.com/owlcs/owlapi
"""

let private dummyChildTtl = """@prefix : <https://nice.org.uk/ontologies/thingy/> .
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

let dummyVocabulary = [
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
    Label = "Applies to thingy"
  }
]

let private dummy_contexts = [ { Prefix = "core"
                                 Value = "https://nice.org.uk/ontologies/core/" }
                               { Prefix = "thingy"
                                 Value = "https://nice.org.uk/ontologies/thingy/" }
                               { Prefix = "whatsit"
                                 Value = "https://nice.org.uk/ontologies/whatsit/" }
                               { Prefix = "xsd"
                                 Value = "http://www.w3.org/2001/XMLSchema#" }
                               { Prefix = "rdfs"
                                 Value = "http://www.w3.org/2000/01/rdf-schema#" } ]

let dummyResponse_Properties = {
  contexts = dummy_contexts
  properties = [
                { id = "core:GUID_stringProperty"
                  label = Some "This thingy"
                  range = None
                  detail = Property { Mandatory = true
                                      Pattern = Some "^qs[1-9]\\d*-st[1-9]\\d*$"
                                      Condition = None }}
                { id = "core:GUID_boolProperty"
                  label = None
                  range = Some "xsd:boolean"
                  detail = Property { Mandatory = true
                                      Pattern = None
                                      Condition = None }}
                { id = "core:GUID_conditionalProperty"
                  label = Some "That thingy changed"
                  range = Some "xsd:date"
                  detail = Property { Mandatory = false
                                      Pattern = None
                                      Condition = Some { OnProperty = "core:GUID_boolProperty"; Value = "no" } }}
              ]
}

let dummyResponse_Vocabs = {
  contexts = dummy_contexts
  properties = [
                 { id = "core:applies_to_thingy"
                   label = Some "Applies to thingy"
                   range = None
                   detail = Tree [
                                   { id = "thingy:thingy_level_1"
                                     label = "Thingy Level 1"
                                     children = [ { id = "thingy:thingy_level_1_1"
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

let private partialJsonResponse_contexts = """{
  "@context": {
    "core": "https://nice.org.uk/ontologies/core/",
    "rdfs": "http://www.w3.org/2000/01/rdf-schema#",
    "thingy": "https://nice.org.uk/ontologies/thingy/",
    "whatsit": "https://nice.org.uk/ontologies/whatsit/",
    "xsd": "http://www.w3.org/2001/XMLSchema#"
  },
  "properties": ["""

let private partialJsonResponse_properties = """
    {
      "@id": "core:GUID_stringProperty",
      "rdfs:label": "This thingy",
      "rdfs:range": "xsd:string",
      "validation": {
        "mandatory": true,
        "pattern": "^qs[1-9]\\d*-st[1-9]\\d*$"
      }
    },
    {
      "@id": "core:GUID_boolProperty",
      "rdfs:label": "GUID_boolProperty",
      "rdfs:range": "xsd:boolean",
      "validation": {
        "mandatory": true
      }
    },
    {
      "@id": "core:GUID_conditionalProperty",
      "rdfs:label": "That thingy changed",
      "rdfs:range": "xsd:date",
      "validation": {
        "condition": {
          "@id": "core:GUID_boolProperty",
          "value": "no"
        }
      }
    }"""

let private partialJsonResponse_vocab = """
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
      "rdfs:label": "Applies to thingy"
    }"""
let partialJsonResponse_end = """
  ]
}"""

let dummyJsonResponse_vocab = sprintf "%s%s%s" partialJsonResponse_contexts partialJsonResponse_vocab partialJsonResponse_end
let dummyJsonResponse_properties = sprintf "%s%s%s" partialJsonResponse_contexts partialJsonResponse_properties partialJsonResponse_end
let dummyJsonResponse_full = sprintf "%s%s,%s%s" partialJsonResponse_contexts partialJsonResponse_properties partialJsonResponse_vocab partialJsonResponse_end

let dummyConfigFile = """{
  "basettluri": "http://schema/ontologies/",
  "baseontologyuri": "https://nice.org.uk/ontologies/",
  "coreontology": {
    "prefix": "core",
    "ontology": "core/",
    "ttl": "core.ttl",
    "dataproperties": [
      { "property": "GUID_stringProperty",
        "validation": {
          "mandatory": true,
          "pattern": "^qs[1-9]\\d*-st[1-9]\\d*$"
        }
      },
      { "property": "GUID_boolProperty",
        "validation": {
          "mandatory": true
        }
      },
      { "property": "GUID_conditionalProperty",
        "validation": {
          "condition": {
            "onproperty": "core:GUID_boolProperty",
            "value": "no"
          }
        }
      }
    ]
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
  ],
  "externalreferences": [
    { "prefix": "xsd",
       "uri": "http://www.w3.org/2001/XMLSchema#"
    },
    { "prefix": "rdfs",
       "uri": "http://www.w3.org/2000/01/rdf-schema#"
    }
  ]
}
"""

let private dummyOntologies = [ { Uri= "core:applies_to_thingy"; SourceTtl= Content dummyChildTtl } ]
let private dummyProperties = [ { PropertyId= "https://nice.org.uk/ontologies/core/GUID_stringProperty"; Detail = { Mandatory=true; Pattern = Some "^qs[1-9]\\d*-st[1-9]\\d*$"; Condition = None }}
                                { PropertyId= "https://nice.org.uk/ontologies/core/GUID_boolProperty"; Detail = { Mandatory=true; Pattern = None; Condition = None }}
                                { PropertyId= "https://nice.org.uk/ontologies/core/GUID_conditionalProperty"; Detail = { Mandatory=false; Pattern = None; Condition = Some { OnProperty = "core:GUID_boolProperty"; Value = "no" }}} ]

let dummyOntologyConfigUri = { CoreTtl= Uri "http://schema/ontologies/core.ttl"
                               Contexts= [ { Prefix="core"; Value= "https://nice.org.uk/ontologies/core/" }
                                           { Prefix="thingy"; Value= "https://nice.org.uk/ontologies/thingy/" }
                                           { Prefix="whatsit"; Value= "https://nice.org.uk/ontologies/whatsit/" }
                                           { Prefix="xsd"; Value= "http://www.w3.org/2001/XMLSchema#" }
                                           { Prefix="rdfs"; Value= "http://www.w3.org/2000/01/rdf-schema#" } ]
                               Ontologies = [ { Uri= "core:applies_to_thingy"; SourceTtl= Uri "http://schema/ontologies/thingy.ttl" } ]
                               Properties = dummyProperties
                             }

let dummyOntologyConfigFull = { dummyOntologyConfigUri with CoreTtl = Content dummyCoreTtl; Ontologies = dummyOntologies }
let dummyOntologyConfigVocab = { dummyOntologyConfigFull with Properties = [] }
let dummyOntologyConfigProperties = { dummyOntologyConfigFull with Ontologies = [] }
//  id: string
//  mandatory: bool
//  pattern: string option
//  condition: propertyCondition option
