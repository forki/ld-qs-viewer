#I "../viewer/bin/Release"
#r "FSharp.RDF.dll"
#r "FSharp.Data.dll"
#r "dotNetRDF.dll"
#r "VDS.Common.dll"
#r "Newtonsoft.Json.dll"
#load "Types.fs"
#load "Utils.fs"

open FSharp.RDF
open VDS.Common
open Viewer.Types
open FSharp.Data
open Viewer.Utils

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
            | None -> "????" //When are we getting this? Needs elaboration

        {Uri = Resource.id x;
         Parents = parents x;
         Label = label x}

    static member walk =
        function
          | {Uri = uri; Parents = xs; Label = label} ->
            (label, uri) :: List.collect Term.walk xs


///Load all resources from uri and make a map of rdfs:label -> resource uri
let vocabGeneration ttl =
    let gcd = Graph.loadTtl (fromString ttl)
    Resource.fromType (Uri.from "http://www.w3.org/2002/07/owl#Class") gcd
    |> List.map Term.from
    |> List.map(Term.walk >> List.rev)
    |> List.concat
    |> Seq.distinct
    |> Seq.toList
    |> List.map(fun ls ->
                match ls with
                | lbl, uri -> {Name = lbl; Uri = string uri; Selected = false})


let vocabLookup uri =
  vocabGeneration(Http.RequestString uri)

let GetVocabs () =
    [{Label = "Settings:"; Name = "setting"; Terms = vocabLookup "http://schema/ns/qualitystandard/setting.ttl"};
    {Label = "Service areas:"; Name = "serviceArea"; Terms = vocabLookup "http://schema/ns/qualitystandard/servicearea.ttl"};
    {Label = "Age groups:"; Name = "targetPopulation"; Terms = vocabLookup "http://schema/ns/qualitystandard/agegroup.ttl"}]


let matchTermsWithQString vocabTerms selected =
  let exists list el = List.exists(fun ele -> ele = el) list
  vocabTerms |> List.map(fun vt ->
                         match vt with
                         | {Name = n; Uri = u; Selected = s} when (exists selected u) -> {Name = n; Uri = u; Selected = true}
                         | {Name = n; Uri = u; Selected = s} -> {Name = n; Uri = u; Selected = false})


let GetVocabsWithState qString =
  let selectedTerms = extractFilters qString
  if (selectedTerms |> List.isEmpty) then
    GetVocabs()
  else
    GetVocabs()
    |> List.map(fun vocab ->
                match vocab with
                | {Label = label; Name = name; Terms = terms} ->
                {Label = label; Name = name; Terms = (matchTermsWithQString terms selectedTerms)})



//Testing
let vTList = [{Name = "Care Home"; Uri = "http://ld.nice.org.uk/test#Care Home"; Selected = false};{Name = "Old Home"; Uri = "http://ld.nice.org.uk/test#Old Home"; Selected = false}]

let selected = ["http://ld.nice.org.uk/test#Care Home"]

matchTermsWithQString vTList selected
