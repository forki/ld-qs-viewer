module Viewer.Components.Search

let extractFilters qs =
  qs
  |> Seq.map (fun (_,v) ->
                match v with
                  | Some s -> s
                  | None -> "")
  |> Seq.toList
