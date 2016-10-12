module Viewer.Types

open FSharp.RDF
open FSharp.Data
//open Viewer.SuaveExtensions
open Viewer.Config

type Filter = {
  Vocab: string
  TermUri: string
}

type AggregatedFilter = {
  Vocab: string
  TermUris: string list
}

type LabelledFilter = {
  VocabLabel: string
  TermUri: string
}

type SearchResult = {
  Uri: string
  Title: string
  Abstract: string
  FirstIssued: System.DateTime 
}

type Tag = {
  Label: string
  RemovalQueryString: string
}

type Component = {
  Content : string
  Script : string
}

//subClassOf relations are the opposite way round from what we need
type InverseTerm =
  { Uri : Uri
    ShortenedUri : string
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
      ShortenedUri = "" 
      Parents = parents x
      Label = label x }

type TermD =
  { Uri : Uri
    ShortenedUri : string
    Label : string
    Selected : bool
    Children : Term list }
and Term =
  | Term of TermD
  | Empty

  //Will need a general implementation for DU templating
  interface DotLiquid.ILiquidizable with
    member __.ToLiquid() =
      match __ with
      | Empty -> null :> obj
      | Term x ->
        let h = DotLiquid.Hash()
        h.Add("uri", (string) x.Uri)
        h.Add("ShortenedUri", x.ShortenedUri)
        h.Add("label", x.Label)
        h.Add("selected", x.Selected)
        h.Add("children", x.Children)
        h :> obj

  static member from xs =
    let rec walk = function
      | { Uri = uri; Label = label; ShortenedUri=""; Parents = [] } -> [ (uri, label) ]
      | { Uri = uri; Label = label; ShortenedUri=""; Parents = xs :: _ } ->
        (uri, label) :: walk xs
    walk xs |> List.fold (fun c (uri, label) ->
                 Term { Uri = uri
                        ShortenedUri = uri.ToString().Replace(BaseUrl, "")
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
                 ShortenedUri = uri.ToString().Replace(BaseUrl, "")
                 Label = label
                 Selected = false
                 Children = (matchingTerms xs ys @ uniqueTerms xs ys) |> sortbyLabel }
    match a, b with
    | Empty, Empty -> Empty
    | Empty, Term x -> Term x
    | Term x, Empty -> Term x
    | Term x, Term y -> combine x y

type Vocabulary = {
  Root : Term
  Property : string
  Label : string
}

type VocabModel = {
  Vocabularies: Vocabulary list
}

type vocabLookup = {
  Label : string
  Guid : string
  ShortUri : string
}