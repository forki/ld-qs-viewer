module Viewer.YamlHandler

open Viewer.Utils
open Viewer.Types
open SharpYaml.Serialization


let YamlBuilder (selected:Filter list) =
    let dict = new System.Collections.Generic.Dictionary<string,System.Collections.Generic.List<string>>()
    let settings =
        SerializerSettings
            (EmitDefaultValues = true, EmitTags = false, SortKeyForMapping = false)

    let grouped = selected |> Seq.groupBy(fun g -> g.Vocab)
    for keys in grouped do
        let list = new System.Collections.Generic.List<string>()
        let k,v = keys
        v |> Seq.iter(fun y ->
                      let term = stripAllButFragment y.TermUri
                      list.Add(term)) |> ignore
        dict.Add(k,list)
        printfn "%A" dict

    let serializer = new Serializer(settings)
    serializer.Serialize(dict)
