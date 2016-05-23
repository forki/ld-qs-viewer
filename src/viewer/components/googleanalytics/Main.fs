module Viewer.Components.GoogleAnalytics

open Viewer.SuaveExtensions

type GAModel = {
  Id: string
}

let createModel id =
  {Id =id} 

let render id = 
  template "components/googleanalytics/index.html" {Id = id}
