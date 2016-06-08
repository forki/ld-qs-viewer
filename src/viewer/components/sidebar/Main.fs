module Viewer.Components.Sidebar

open Viewer.SuaveExtensions

type SidebarModel = {
  Vocabularies: string
}

let render renderedVocabs =
  {Vocabularies = renderedVocabs} 
  |> template "components/sidebar/index.html"
