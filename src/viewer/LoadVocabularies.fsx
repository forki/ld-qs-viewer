#I "../viewer/bin/Release"
#r "FSharp.RDF.dll"
#r "dotNetRDF.dll"
#r "VDS.Common.dll"
#r "Newtonsoft.Json.dll"

open FSharp.RDF
type Term = {
  Uri : Uri
  Label : string 
  Parents : Term list
  }
with static member from x =
  let parents x =
    match (|Property|_|) ( Uri.from "http://www.w3.org/2000/01/rdf-schema#subClassOf" ) x with
      | None -> []
      | Some xs -> traverse xs |> List.map Term.from

  let label x =
    match (|FunctionalDataProperty|_|) (Uri.from "http://www.w3.org/2000/01/rdf-schema#label" ) (xsd.string) x with
      | Some x -> x
      | _ -> "????"
  {
    Uri = Resource.id x
    Parents = parents x
    Label = label x
  }
  static member walk = function
    | {Uri = uri
       Parents = xs
       Label = label} -> uri::List.collect Term.walk xs

///Load all resources from uri and make a map of rdfs:label -> resource uri
let vocabLookup uri =
  let gcd = Graph.loadFrom uri
  printf "%s" ( Graph.print gcd )
  Resource.fromType (Uri.from "http://www.w3.org/2002/07/owl#Class") gcd
  |> List.map Term.from
  |> List.map (Term.walk >> List.rev >> List.map string)

  //  Resource.fromPredicate rdfslbl gcd
  //|> List.map (fun r ->
    //   match r with
      // | FunctionalDataProperty rdfslbl xsd.string x -> printfn "%A" x
       //| r -> printfn "%A" r)

//"http://www.w3.org/2000/01/rdf-schema#subClassOf"
// "yhttp://www.w3.org/2000/01/rdf-schema#label"

