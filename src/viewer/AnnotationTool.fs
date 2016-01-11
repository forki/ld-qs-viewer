module Viewer.AnnotationTool

open Suave
open Suave.Types
open Viewer.Model
open Viewer.Utils
open Viewer.VocabGeneration
open Viewer.Components
open Viewer.Components.AnnotationSidebar
open Viewer.Components.AnnotationBlock

type AnnotationToolModel = {
  blah1 : AnnotationSidebarModel    
  blah2 : AnnotationBlockModel
}

let annotationTool (req:HttpRequest) config (convert:bool) =

  {blah1 = AnnotationSidebar.createModel req config.Vocabs
   blah2 = AnnotationBlock.createModel req config.Vocabs convert}
  |> DotLiquid.page "templates/annotationtool.html"
