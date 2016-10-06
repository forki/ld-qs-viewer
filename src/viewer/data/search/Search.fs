module Viewer.Data.Search.Search

open Viewer.Types
open Serilog

let logSearch filters (results:SearchResult list) = 
  let vocabs = filters |> List.map (fun {Vocab=v} -> v)
  let terms = filters |> List.collect (fun {TermUris=t} -> t)
  Log.Information("Search with vocabs: {@vocabs}, terms: {@terms}, resultCount: {@resultCount}", vocabs, terms, results.Length)
  results

let performSearchWithProvider searchProviderFn filters =
  filters
  |> searchProviderFn
  |> logSearch filters
