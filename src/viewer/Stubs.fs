module Stubs
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.RDF


let vocabs = [{Root = Term {Uri = (Uri.from "http://ld.nice.org.uk/ns/qualitystandard/setting")
                            ShortenedUri = "setting"
                            Label = "Settings"
                            Selected = false
                            Children = [
                                         Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting/long-guid-1"
                                                ShortenedUri = "TestSetting/long-guid-1"
                                                Label = "ARRGHGHGHHGHGHGH"
                                                Selected = false
                                                Children = [
                                                             Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting#Term1A"
                                                                    ShortenedUri = "TestSetting/long-guid-1A"
                                                                    Label = "Term1A"
                                                                    Selected = false
                                                                    Children = [
                                                                                 Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting#Term1 AA"
                                                                                        ShortenedUri = "TestSetting/long-guid-AA"
                                                                                        Label = "Term1 A A"
                                                                                        Selected = false
                                                                                        Children = []};
                                                                   ]};
                                                      Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting1#Term1+ B"
                                                             ShortenedUri = "TestSetting/long-guid-1B"
                                                             Label = "Term1+ B"
                                                             Selected = false
                                                             Children = []};
                                                ]};
                                         Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting#Term2"
                                                ShortenedUri = "TestSetting/Term2"
                                                Label = "Term2"
                                                Selected = false
                                                Children = []};
                                       ]};
              Property = "qualitystandard:setting";
              Label = "Setting"}
              {Root = Term {Uri = (Uri.from "http://ld.nice.org.uk/ns/qualitystandard/ServiceArea")
                            ShortenedUri = "area"
                            Label = "Service Area"
                            Selected = false
                            Children = [
                                          Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestArea#Term1"
                                                 ShortenedUri = "TestArea/long-guid-3"
                                                 Label = "Term3"
                                                 Selected = false
                                                 Children = []};
                                          Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestArea#Term2"
                                                 ShortenedUri = "TestArea/long-guid-4"
                                                 Label = "Term4"
                                                 Selected = false
                                                 Children = []};]};
              Property = "qualitystandard:serviceArea";
              Label = "Service Area"}]

let getSearchResults _ _ = [{Uri = "Uri1"; Abstract = "Unicorns under the age of 65..."; Title = "This is the title"; FirstIssued = new System.DateTime()};
                            {Uri = "Uri2"; Abstract = "Goblins with arthritis..."; Title = "Quality standard xxx from quality statement xxx"; FirstIssued = new System.DateTime()};]
                            
let search _ = [{Uri = "Uri1"; Abstract = "Unicorns under the age of 65..."; Title = "This is the title"; FirstIssued = new System.DateTime()};
                {Uri = "Uri2"; Abstract = "Goblins with arthritis..."; Title = "Quality standard xxx from quality statement xxx"; FirstIssued = new System.DateTime()};]

let getKBCount _ = 0
