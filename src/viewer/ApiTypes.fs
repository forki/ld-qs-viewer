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

type OntologyReference =
  {
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

type propertyCondition =
  {
    OnProperty: string
    Value: string
  }
  static member ToJson (x:propertyCondition) =
    Json.write "@id" x.OnProperty
    *> Json.write "value" x.Value

type CorePropertyDetail =
  {
    Mandatory: bool
    Condition: propertyCondition option
    Default: string option
  }
  static member  ToJson (x:CorePropertyDetail) =
    Json.writeUnlessDefault "mandatory" false x.Mandatory
    *> Json.writeUnlessDefault "condition" None x.Condition
    *> Json.writeUnlessDefault "default" None x.Default

type CoreProperty =
  {
    PropertyId: string
    Detail: CorePropertyDetail option
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
        |> List.map ( fun x -> { Prefix = x.Prefix; Value = ontologyUri x.Ontology })) @
      ( d.Externalreferences
        |> Array.toList
        |> List.map ( fun x -> { Prefix = x.Prefix; Value = x.Uri } ))
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
      |> List.map (fun x -> { PropertyId=(sprintf "%s%s%s" "http://dev:20002/ontologies/" d.Coreontology.Ontology x.Property)
                              Detail= match x.Validation with
                                      | None -> None
                                      | Some y -> Some { Mandatory=(getboolvalue y.Mandatory)
                                                         Default=y.Default
                                                         Condition=match y.Condition with
                                                                   | Some z -> Some { OnProperty = z.Onproperty; Value = z.Value }
                                                                   | _ -> None }
                            })

    printf "%A" getProperties
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
  | Property of CorePropertyDetail option

type OntologyResponseProperty =
  {
    id: string
    label: string option
    range: string option
    pattern: string option
    example: string option
    detail: OntologyResponseType
  }
  static member empty =
    { id = ""
      label = None
      range = None 
      pattern = None
      example = None
      detail = Property None
    }
  static member ToJson (x:OntologyResponseProperty) =
    let getLabel =
      match x.label with
      | None -> Array.get ((x.id.Split [|':'|]) |> Array.rev) 0
      | Some l -> l
    let getRange =
      match x.detail with
      | Tree _ -> x.range
      | Property _ -> match x.range with
                      | None -> Some "xsd:string"
                      | _ -> x.range
    let getValidation =
      match x.detail with
      | Tree _ -> None
      | Property p -> p

    let ret = Json.write "@id" x.id
              *> Json.write "rdfs:label" getLabel
              *> Json.writeUnlessDefault "xsd:pattern" None x.pattern
              *> Json.writeUnlessDefault "skos:example" None x.example
              *> Json.writeUnlessDefault "rdfs:range" None getRange
              
    match x.detail with
    | Tree t -> ret *> Json.writeUnlessDefault "options" [] t
    | Property _ -> ret *> Json.writeUnlessDefault "validation" None getValidation

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
    ConstructJson (ToJson x.Contexts.Head) x.Contexts.Tail

type OntologyResponse =
  {
    contexts : Context List
    properties : OntologyResponseProperty list
  }  
  static member ToJson (x:OntologyResponse) =
    Json.write "@context" (Contexts.build x.contexts)
    *> Json.write "properties" x.properties
