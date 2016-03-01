module Viewer.Components.Hotjar

open Viewer.SuaveExtensions

type HotjarModel = {
    Id: string
}

let createModel id =
    {Id =id} 

let Build id = 
  template "components/hotjar/index.html" {Id = id}
