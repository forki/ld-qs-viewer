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

type propertyCondition = {
  onproperty: string
  value: string
}

type CoreProperty = {
  id: string
  mandatory: bool
  pattern: string option
  condition: propertyCondition option
}

type OntologyConfig =
  {
    CoreTtl : Ttl
    Contexts : Context list
    Ontologies : OntologyReference list
    Properties : CoreProperty list
  }
  static member build data =
    let getstringvalue x =
      match x with
      | Some v -> v
      | _ -> ""
    
    let getboolvalue x =
      match x with
      | Some v -> v
      | _ -> false  
    
    let d = JsonProvider<"data/config/configSchema.json">.Parse(data)
    
    let getContexts = 
      let ontologyUri x = (sprintf "%s%s" d.Baseontologyuri x)
      [{ Prefix = d.Coreontology.Prefix; Value = ontologyUri d.Coreontology.Ontology }] @ 
      ( d.Childontologies
        |> Array.toList
        |> List.map ( fun x -> { Prefix = x.Prefix; Value = ontologyUri x.Ontology }))
    let getOntologies =
      d.Childontologies
      |> Array.toList
      |> List.filter (fun x -> x.Corereference.IsSome)
      |> List.map (fun x -> { Uri=(sprintf "%s:%s" d.Coreontology.Prefix (getstringvalue x.Corereference));
                              SourceTtl = Uri (sprintf "%s%s" d.Basettluri (getstringvalue x.Ttl))
                            })
    let getProperties =
      d.Coreontology.Dataproperties
      |> Array.toList
      |> List.map (fun x -> { id=x.Property
                              mandatory=(getboolvalue x.Validation.Mandatory)
                              pattern=x.Validation.Pattern
                              condition=match x.Validation.Condition with
                                              | Some y -> Some { onproperty = y.Onproperty; value = y.Value }
                                              | _ -> None
                            })

    {
      CoreTtl = Uri (sprintf "%s%s" d.Basettluri d.Coreontology.Ttl)
      Contexts = getContexts
      Ontologies = getOntologies
      Properties = getProperties
    }

let emptyOC = { CoreTtl = Content ""; Contexts= []; Ontologies=[]; Properties=[] }

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

type OntologyResponseType =
  | Tree of OntologyTreeOption list
  | Property of CoreProperty

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
