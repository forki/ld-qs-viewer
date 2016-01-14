module Viewer.Pages.AnnotationTool

open Suave
open Suave.Types
open Viewer.Utils
open Viewer.AppConfig
open Viewer.Components
open Viewer.Components.AnnotationSidebar
open Viewer.Components.AnnotationBlock

type AnnotationToolModel = {
  blah1 : AnnotationSidebarModel    
  blah2 : AnnotationBlockModel
}

let page (req:HttpRequest) config (convert:bool) =

  {blah1 = AnnotationSidebar.createModel req config.Vocabs
   blah2 = AnnotationBlock.createModel req config.Vocabs convert}
  |> DotLiquid.page "templates/annotationtool.html"
