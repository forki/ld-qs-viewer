module Stubs
open Viewer.Types
open Viewer.VocabGeneration
open FSharp.RDF

let vocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/setting")
                            Label = "Settings:"
                            Selected = false
                            Children = [
                                         Term { Uri = Uri.from "http://testing.com/TestSetting1#Term1"
                                                Label = "Term1"
                                                Selected = false
                                                Children = []};
                                         Term { Uri = Uri.from "http://testing.com/TestSetting2#Term2"
                                                Label = "Term2"
                                                Selected = false
                                                Children = []};]};
              Property = "Settings:"}]

let getSearchResults _ _ = [{Uri = "Uri1"; Abstract = "Unicorns under the age of 65..."; Title = "This is the title"};
                            {Uri = "Uri2"; Abstract = "Goblins with arthritis..."; Title = "This is the title"}]

let vocabsForTests = [{Root = Term {Uri = (Uri.from "http://testing.com/setting")
                                    Label = "Settings:"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/TestSetting1"
                                                        Label = "Term1"
                                                        Selected = false
                                                        Children = []};
                                                 Term { Uri = Uri.from "http://testing.com/TestSetting2"
                                                        Label = "Term2"
                                                        Selected = false
                                                        Children = []};]};
                       Property = "qualitystandard:setting"};
                      {Root = Term {Uri = (Uri.from "http://testing.com/ServiceArea")
                                    Label = "Service Area:"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/TestArea1"
                                                        Label = "Term3"
                                                        Selected = false
                                                        Children = []};
                                                 Term { Uri = Uri.from "http://testing.com/TestArea2"
                                                        Label = "Term4"
                                                        Selected = false
                                                        Children = []};]};
                       Property = "qualitystandard:serviceArea"}]

let getKBCount _ = 0
