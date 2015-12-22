#r "/Users/Nate/_src/ld-freya/bin/SharpYaml.dll"
#r "/Users/Nate/_src/ld-freya/bin/owlapi.dll"
#r "/Users/Nate/_src/ld-freya/bin/Newtonsoft.Json.dll"
#r "/Users/Nate/_src/ld-freya/bin/JsonLD.dll"
#r "/Users/Nate/_src/ld-freya/packages/FSharp.Data/lib/net40/FSharp.Data.DesignTime.dll"
#r "/Users/Nate/_src/ld-freya/packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "/Users/Nate/_src/ld-freya/packages/FSharp.RDF/lib/IKVM.OpenJDK.Core.dll"
#r "/Users/Nate/_src/ld-freya/packages/FSharp.RDF/lib/dotNetRDF.dll"
#r "/Users/Nate/_src/ld-freya/packages/FSharp.RDF/lib/FSharp.RDF.dll"
#r "/Users/Nate/_src/ld-freya/packages/FSharp.RDF/lib/HtmlAgilityPack.dll"
#r "/Users/Nate/_src/ld-viewer/bin/viewer/Suave.dll"
#r "/Users/Nate/_src/ld-viewer/bin/viewer/DotLiquid.dll"
#r "/Users/Nate/_src/ld-viewer/bin/viewer/Suave.DotLiquid.dll"
#r "/Users/Nate/_src/ld-viewer/bin/viewer/viewer.dll"
#load "/Users/Nate/_src/ld-freya/src/freya/Yaml.fs"

open Viewer.VocabGeneration
open Viewer.Types
open FSharp.Data
open FSharp.RDF
open Freya.YamlParser
open SharpYaml.Serialization

type Vocabulary = {
  Root : Term
  Property : string
  }

type Selected = {
  Property : string
  Values : string list
  }

let lookupVocab =
  ([ "setting",
     vocabLookup "http://schema/ns/qualitystandard/setting.ttl"

     "agegroup",
     vocabLookup "http://schema/ns/qualitystandard/agegroup.ttl"

     "lifestylecondition",
     vocabLookup
       "http://schema/ns/qualitystandard/lifestylecondition.ttl"

     "conditiondisease",
     vocabLookup "http://schema/ns/qualitystandard/conditiondisease.ttl"

     "servicearea",
     vocabLookup "http://schema/ns/qualitystandard/servicearea.ttl" ]
   |> Map.ofList)


// Yaml -> Vocab


let yaml = """
Setting:
    - Care home
    - "Community"
Age Group:
    - "Adults"
    - "Young people 16-17 years"
    - "Children 1-15 years"
Condition Disease:
    - "Learning disabilities"
Service area:
    - "Social care"
    - "Mental health care"
"""

let parsed = parse yaml

let uriBuilder term vocab =
    System.Uri.EscapeUriString (sprintf "http://ld.nice.org.uk/ns/qualitystandard/%s#%s" vocab term)

let extractScalar x vocab =
    match x with
        | Node.Scalar(Scalar.String s) -> (uriBuilder s vocab)

let extractList x vocab =
    match x with
        | Node.List lxs -> lxs |> List.map(fun xs -> extractScalar xs vocab)

let extractMap x =
    match x with
        | Node.Map map ->
            map |> List.map(fun m -> m)

let rec yamlExtractor node acc =
    match node with
        | [] -> acc
        | x::xs ->
            let rootTerm, scalars = x
            let lst = {Property = (rootTerm); Values = extractList scalars rootTerm}
            lst :: yamlExtractor xs acc

let data = yamlExtractor (extractMap parsed) []


// Vocab -> Yaml

let YamlBuilder (selected:Filter list) =
    let dict = new System.Collections.Generic.Dictionary<string,System.Collections.Generic.List<string>>()
    let settings =
        SerializerSettings
            (EmitDefaultValues = true, EmitTags = false, SortKeyForMapping = false)

    let grouped = selected |> Seq.groupBy(fun g -> g.Vocab)
    for keys in grouped do
        let list = new System.Collections.Generic.List<string>()
        let k,v = keys
        v |> Seq.iter(fun y -> list.Add(y.TermUri)) |> ignore
        dict.Add(k,list)
        printfn "%A" dict

    let serializer = new Serializer(settings)
    serializer.Serialize(dict)











let list = new System.Collections.Generic.List<string>()
let dict = new System.Collections.Generic.Dictionary<string,System.Collections.Generic.List<string>>()
list.Add("Care Home")
list.Add("Community")

dict.Add("Setting", list)
let settings =
    SerializerSettings
        (EmitDefaultValues = true, EmitTags = false, SortKeyForMapping = false)
let serializer = new Serializer(settings)
let text = serializer.Serialize(dict)
