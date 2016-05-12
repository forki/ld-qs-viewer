module Viewer.Pages.AnnotationTool

open Suave
open Viewer.Utils
open Viewer.AppConfig
open Viewer.Components
open Viewer.Components.AnnotationBlock

type AnnotationToolModel = {
  Vocabularies : string
  blah2 : AnnotationBlockModel
}

let page (req:HttpRequest) config (convert:bool) =

  {Vocabularies = config.RenderedVocabs
   blah2 = AnnotationBlock.createModel req config.Vocabs convert}
  |> DotLiquid.page "templates/annotationtool.html"
