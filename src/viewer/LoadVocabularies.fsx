#I "../viewer/bin/Release"
#r "FSharp.RDF.dll"
#r "dotNetRDF.dll"
#r "VDS.Common.dll"
#r "Newtonsoft.Json.dll"
#r "FSharp.Data.dll"
#load "Types.fs"
#load "Utils.fs"

open FSharp.RDF
open Viewer.Types

//subClassOf relations are the opposite way round from what we need
type InverseTerm =
  { Uri : Uri
    Label : string
    Parents : InverseTerm list }
  static member from lbl x =
    let parents x =
      match (|Property|_|)
              (Uri.from "http://www.w3.org/2000/01/rdf-schema#subClassOf") x with
      | None -> []
      | Some xs -> traverse xs |> List.map (InverseTerm.from lbl)

    let label x =
      match (|FunctionalDataProperty|_|)
              (Uri.from "http://www.w3.org/2000/01/rdf-schema#label")
              (xsd.string) x with
      | Some x -> x
      | _ -> lbl

    { Uri = Resource.id x
      Parents = parents x
      Label = label x }

type TermD =
  { Uri : Uri
    Label : string
    Selected : bool
    Children : Term list }
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
                        Selected = false
                        Children =
                          (if c = Empty then []
                           else [ c ]) }) Empty

  static member (++) (a, b) =
    let combine a b =
      match a, b with
      | { Uri = uri; Label = label; Children = xs },
        { Uri = uri'; Children = ys } ->
        let sortbyLabel = List.sortBy (function
                                       | Term {Label=label} -> label
                                       | _ -> "") 
        //Recursively merge terms that are present in both
        let matchingTerms xs ys =
          let termFrom x y =
            match x, y with
            | Term x, Term y when x.Uri = y.Uri -> Some(Term x ++ Term y)
            | _ -> None
          [ for x in xs -> (List.tryPick (termFrom x) ys) ]
          |> List.choose id

        //Return terms that are only on one side or the other
        let uniqueTerms xs ys =
          let termHasUri x y =
            match x, y with
            | Term x, Term y -> x.Uri = y.Uri
            | _ -> false
          [ for x in xs do
              if not (List.exists (termHasUri x) ys) then yield x ]
          @ [ for y in ys do
                if not (List.exists (termHasUri y) xs) then yield y ]

        match uri = uri' with
        | false -> Term a
        | true ->
          Term { Uri = uri
                 Label = label
                 Selected = false
                 Children = (matchingTerms xs ys @ uniqueTerms xs ys) |> sortbyLabel }
    match a, b with
    | Empty, Empty -> Empty
    | Empty, Term x -> Term x
    | Term x, Empty -> Term x
    | Term x, Term y -> combine x y

///Load all resources from uri and make a map of rdfs:label -> resource uri
let vocabGeneration ttl lbl =
  let gcd = Graph.loadTtl (fromString ttl)
  Resource.fromType (Uri.from "http://www.w3.org/2002/07/owl#Class") gcd
  |> List.map (InverseTerm.from lbl)
  |> List.map Term.from
  |> List.fold (++) Empty

let vocabLookup uri lbl = vocabGeneration (FSharp.Data.Http.RequestString uri) lbl

type Vocabulary = {
  Root : Term
  Property : string
}

let readVocabsFromFiles () =
  [
    {
      Root = vocabLookup "http://ld.nice.org.uk/ns/qualitystandard/setting.ttl" "Setting"
      Property = "qualitystandard:setting"
    }
    {
      Root = vocabLookup "http://ld.nice.org.uk/ns/qualitystandard/agegroup.ttl" "Age group"
      Property = "qualitystandard:age"
    }
    {
      Root = vocabLookup "http://ld.nice.org.uk/ns/qualitystandard/servicearea.ttl" "Service area"
      Property = "qualitystandard:serviceArea"
    }
    {
      Root = vocabLookup "http://ld.nice.org.uk/ns/qualitystandard/conditiondisease.ttl" "Condition or disease"
      Property = "qualitystandard:condition"
    }
    {
      Root = vocabLookup "http://ld.nice.org.uk/ns/qualitystandard/lifestylecondition.ttl" "Lifestyle condition"
      Property = "qualitystandard:lifestylecondition"
    }
  ]

let setSelectedIfFiltered filters vocab =

  let rec filterChildren f acc children =
    match children with
      | [] -> []
      | x::xs -> f x :: filterChildren f acc xs

  let rec filterVocab v =
    match v with
    | Empty -> v
    | Term x -> Term { x with
                        Selected = filters |> Seq.exists (fun y -> y = x.Uri.ToString())
                        Children = filterChildren filterVocab [] x.Children }

  {vocab with Root = filterVocab (vocab.Root)}

let getVocabsWithState vocabs filters =
  vocabs
  |> Seq.map (fun v -> setSelectedIfFiltered filters v)
  |> Seq.toList


readVocabsFromFiles ()
