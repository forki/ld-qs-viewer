module Viewer.Resource

open Suave
open Suave.Types
open System.IO

type ResourceModel = {
  Content : string
}

let resource filename =
  let content =
    try
      File.ReadAllText(sprintf "/artifacts/published/qualitystandards/%s" filename)
    with
      | ex -> "Could not find resource."

  DotLiquid.page "resource.html" {Content = content}

