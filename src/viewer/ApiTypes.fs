module Viewer.ApiTypes

open Chiron
open Chiron.Operators

type Result<'TSuccess> = 
  | Success of 'TSuccess
  | Failure of string

type Ttl =
  | Uri of string
  | Content of string

type OntologyReference = {
  Uri : string
  SourceTtl : Ttl
}

type Context =
  {
    Prefix : string
    Value: string
  }
  static member ToJson (x:Context) =
    Json.write x.Prefix x.Value
    

type OntologyConfig = {
  CoreTtl : Ttl
  Contexts : Context list
  Predicates : OntologyReference list
}

let emptyOC = { CoreTtl = Content ""; Contexts= []; Predicates=[] }

type OntologyTreeOption =
  {
    id: string
    label: string
    children: OntologyTreeOption List
  }
  static member ToJson (x:OntologyTreeOption) =
    Json.write "@id" x.id
    *> Json.write "rdfs:label" x.label
    *> Json.writeUnlessDefault "children" [] x.children

type OntologyResponseProperty =
  {
    id: string
    label: string
    options: OntologyTreeOption list
  }
  static member ToJson (x:OntologyResponseProperty) =
    Json.write "@id" x.id
    *> Json.write "rdfs:label" x.label
    *> Json.writeUnlessDefault "options" [] x.options

type OntologyResponse =
  {
    contexts : Context List
    properties : OntologyResponseProperty list
  }
  static member ToJson (x:OntologyResponse) =
    Json.write "@context" x.contexts
    *> Json.write "properties" x.properties