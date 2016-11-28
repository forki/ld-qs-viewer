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
    let core = d.Details |> Array.find (fun x -> x.Core)
    let prefixes = d.Details |> Array.toList
    let predicates = prefixes |> List.filter (fun x -> match x.Corereference with | Some _ -> true | _ -> false && x.Core = false)
    {
      CoreTtl = Uri (sprintf "%s%s" d.Basettl ( core |> (fun x -> getvalue x.Ttl)))
      Contexts = prefixes |> List.map (fun x -> { Prefix = x.Prefix; Value = (sprintf "%s%s" d.Baseontology x.Ontology)})
      Predicates = predicates |> List.map (fun x -> { Uri=(sprintf "%s:%s" core.Prefix (getvalue x.Corereference)); SourceTtl = Uri (sprintf "%s%s" d.Basettl (getvalue x.Ttl))})
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
