module Viewer.Pages.Resource

open Suave
open Suave.Cookie
open System.IO

type ResourceModel = {
  Content : string
}

let private artifacts testing filename =
  match testing with
    | true -> sprintf "/test_artifacts/published/qualitystandards/%s" filename
    | false -> sprintf "/artifacts/published/qualitystandards/%s" filename

let page (request:HttpRequest) filename =
  let testing = request.cookies |> Map.containsKey "test"
  let content =
    try
      File.ReadAllText((artifacts testing filename))
    with
      | ex -> "Could not find resource."

  DotLiquid.page "templates/resource.html" {Content = content}

