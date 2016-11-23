module Viewer.Data.Annotations.AnnotationEndpoint

open Viewer.Components.AnnotationBlock
open Viewer.Types

let toGuidBlock req vocabs =
  let model = createModel req vocabs true
  model.AnnotationBlock
