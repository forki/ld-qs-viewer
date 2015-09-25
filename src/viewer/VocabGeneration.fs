module Viewer.VocabGeneration

open FSharp.RDF
open VDS.Common
open Viewer.Types

type Term =
    {Uri : Uri;
     Label : string;
     Parents : Term list}

    static member from x =
        let parents x =
            match (|Property|_|)
                      (Uri.from
                           "http://www.w3.org/2000/01/rdf-schema#subClassOf") x with
            | None -> []
            | Some xs -> traverse xs |> List.map Term.from

        let label x =
            match (|FunctionalDataProperty|_|)
                      (Uri.from "http://www.w3.org/2000/01/rdf-schema#label")
                      (xsd.string) x with
            | Some x -> x
            | _ -> "????"

        {Uri = Resource.id x;
         Parents = parents x;
         Label = label x}

    static member walk =
        function
        | {Uri = uri; Parents = xs; Label = label} ->
            (label, uri) :: List.collect Term.walk xs

///Load all resources from uri and make a map of rdfs:label -> resource uri
let vocabLookup uri =
    let gcd = Graph.loadFrom uri
    Resource.fromType (Uri.from "http://www.w3.org/2002/07/owl#Class") gcd
    |> List.map Term.from
    |> List.map(Term.walk >> List.rev)
    |> List.concat
    |> Seq.distinct
    |> Seq.toList
    |> List.map(fun ls ->
                match ls with
                | lbl, uri -> {Name = lbl; Uri = string uri})
