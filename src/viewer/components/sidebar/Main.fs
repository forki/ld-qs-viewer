module Viewer.Components.Sidebar

open Viewer.SuaveExtensions

type SidebarModel = {
  Vocabularies: string
  RelevancyTest: int
}

let render renderedVocabs relevancyTest =
  {Vocabularies = renderedVocabs
   RelevancyTest = relevancyTest
  } 
  |> template "components/sidebar/index.html"
