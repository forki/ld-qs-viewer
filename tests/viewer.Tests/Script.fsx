#load "LoadDeps.fsx"

open Swensen.Unquote

let extractFilters qs =
  qs
  |> Seq.map (fun (_,v) ->
                match v with
                  | Some s -> s
                  | None -> "")
  |> Seq.toList

let noResults () =
  let qs = []
  let filters = extractFilters qs
  test <@ filters = [] @>

let results () =
  let qs = [("key1", Some("val1"));
            ("key2", Some("val2"))]
  let filters = extractFilters qs

  test <@ filters = ["val1";"val2"] @>

[noResults; results] |> Seq.iter (fun test -> test())
