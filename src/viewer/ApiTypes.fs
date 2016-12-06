module Viewer.ApiTypes

open Chiron
open Chiron.Operators
open FSharp.Data

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

type OntologyConfig =
  {
    CoreTtl : Ttl
    Contexts : Context list
    Predicates : OntologyReference list
  }
  static member build data =
    let getvalue x =
      match x with
      | Some v -> v
      | _ -> ""
    
    let d = JsonProvider<"data/config/configSchema.json">.Parse(data)
    
    let getContexts = 
      let ontologyUri x = (sprintf "%s%s" d.Baseontologyuri x)
      [{ Prefix = d.Coreontology.Prefix; Value = ontologyUri d.Coreontology.Ontology }] @ 
      ( d.Childontologies
        |> Array.toList
        |> List.map ( fun x -> { Prefix = x.Prefix; Value = ontologyUri x.Ontology }))
    let getPredicates_ontologies =
      d.Childontologies
      |> Array.toList
      |> List.filter (fun x -> x.Corereference.IsSome)
      |> List.map (fun x -> { Uri=(sprintf "%s:%s" d.Coreontology.Prefix (getvalue x.Corereference));
                              SourceTtl = Uri (sprintf "%s%s" d.Basettluri (getvalue x.Ttl))
                            })
    let getPredicates_properties =
      d.Coreontology.Dataproperties
      |> Array.toList
      |> List.map (fun x -> x.Validation.Pattern )

    {
      CoreTtl = Uri (sprintf "%s%s" d.Basettluri d.Coreontology.Ttl)
      Contexts = getContexts
      Predicates = getPredicates_ontologies
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

type propertyCondition = {
  onproperty: string
  value: string
}

type OntologyPropertyValidation = {
  mandatory: bool
  pattern: string option
  condition: propertyCondition option
}

type OntologyResponseType =
  | Tree of OntologyTreeOption list
  | Validation of OntologyPropertyValidation

type OntologyResponseProperty =
  {
    id: string
    label: string
    details: OntologyResponseType
//    options: OntologyTreeOption list
//    validation: OntologyPropertyValidation
  }
  static member ToJson (x:OntologyResponseProperty) =
    let ret = Json.write "@id" x.id
              *> Json.write "rdfs:label" x.label
    
    match x.details with
    | Tree t -> ret *> Json.writeUnlessDefault "options" [] t
    | _ -> ret

type Contexts =
  {
    Contexts: Context List
  }
  static member build data = 
    { Contexts = data }
  static member ToJson (x:Contexts) =
    let ToJson x = Json.write x.Prefix x.Value
    let rec ConstructJson acc (contexts:Context list) =
      match contexts with
      | [] -> acc
      | _ -> ConstructJson (acc *> (contexts.Head |> ToJson)) contexts.Tail
    let rdfs = { Prefix = "rdfs"; Value="http://www.w3.org/2000/01/rdf-schema#" }
    ConstructJson (ToJson rdfs) x.Contexts

type OntologyResponse =
  {
    contexts : Context List
    properties : OntologyResponseProperty list
  }  
  static member ToJson (x:OntologyResponse) =
    Json.write "@context" (Contexts.build x.contexts)
    *> Json.write "properties" x.properties
