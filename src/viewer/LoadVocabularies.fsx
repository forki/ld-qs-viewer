#I "../viewer/bin/Release"
#r "FSharp.RDF.dll"
#r "dotNetRDF.dll"
#r "VDS.Common.dll"
#r "Newtonsoft.Json.dll"
#load "Types.fs"
#load "Utils.fs"

open FSharp.RDF
open Viewer.Types

//subClassOf relations are the opposite way round from what we need
type InverseTerm =
  { Uri : Uri
    Label : string
    Parents : InverseTerm list }
  static member from x =
    let parents x =
      match (|Property|_|)
              (Uri.from "http://www.w3.org/2000/01/rdf-schema#subClassOf") x with
      | None -> []
      | Some xs -> traverse xs |> List.map InverseTerm.from

    let label x =
      match (|FunctionalDataProperty|_|)
              (Uri.from "http://www.w3.org/2000/01/rdf-schema#label")
              (xsd.string) x with
      | Some x -> x
      | _ -> "????"

    { Uri = Resource.id x
      Parents = parents x
      Label = label x }

//This one is the right way round
type TermD =
  { Uri : Uri
    Label : string
    Children : Set<Term> }

and Term =
  | Term of TermD
  | Empty

  static member from xs =
    let rec walk =
      function
      | { Uri = uri; Label = label; Parents = [] } -> [ (uri, label) ]
      | { Uri = uri; Label = label; Parents = xs :: _ } ->
        (uri, label) :: walk xs
    walk xs |> List.fold (fun c (uri, label) ->
                 Term { Uri = uri
                        Label = label
                        Children =
                          (if c = Empty then Set.empty
                           else Set.ofList [ c ]) }) Empty

  static member (++) (a, b) =
    let combine a b =
      match a, b with
      | { Uri = uri; Label = label; Children = xs },
        { Uri = uri'; Label = label'; Children = ys } ->
        match uri = uri', Set.difference ys xs |> Set.toList with
        | false, _ -> Term a
        | _, [] ->
          Term { Uri = uri
                 Label = label
                 Children = Seq.map2 (++) xs ys |> Set.ofSeq }
        | _, d ->
          Term { Uri = uri
                 Label = label
                 Children = Set.union xs ys }
    match a, b with
    | Empty, Empty -> Empty
    | Empty, Term x -> Term x
    | Term x, Empty -> Term x
    | Term x, Term y -> combine x y

///Load all resources from uri and make a map of rdfs:label -> resource uri
let vocabLookup uri =
  let gcd = Graph.loadFrom uri
  Resource.fromType (Uri.from "http://www.w3.org/2002/07/owl#Class") gcd
  |> List.map InverseTerm.from
  |> List.map Term.from
  |> List.fold (++) Empty

vocabLookup "http://ld.nice.org.uk/ns/qualitystandard/agegroup.ttl"
