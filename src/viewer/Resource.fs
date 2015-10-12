module Viewer.Resource

open Suave
open Suave.Types
open System.IO

type ResourceModel = {
  Content : string
}

let resource filename =
  let content = File.ReadAllText(sprintf "/artifacts/published/qualitystandards/%s" filename)
  DotLiquid.page "resource.html" {Content = content}

