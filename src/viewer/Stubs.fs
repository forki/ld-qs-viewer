module Stubs

open Viewer.VocabGeneration
open FSharp.RDF

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
