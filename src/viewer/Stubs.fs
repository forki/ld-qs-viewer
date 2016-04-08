module Stubs
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.RDF


let vocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/setting")
                            ShortenedUri = "setting"
                            Label = "Settings:"
                            Selected = false
                            Children = [
                                         Term { Uri = Uri.from "http://testing.com/TestSetting1#Term1"
                                                ShortenedUri = "setting"
                                                Label = "Term1"
                                                Selected = false
                                                Children = [
                                                             Term { Uri = Uri.from "http://testing.com/TestSetting1#Term1A"
                                                                    ShortenedUri = "setting"
                                                                    Label = "Term1A"
                                                                    Selected = false
                                                                    Children = [
                                                                                 Term { Uri = Uri.from "http://testing.com/TestSetting1#Term1AA"
                                                                                        ShortenedUri = "setting"
                                                                                        Label = "Term1AA"
                                                                                        Selected = false
                                                                                        Children = []};
                                                                   ]};
                                                      Term { Uri = Uri.from "http://testing.com/TestSetting1#Term1B"
                                                             ShortenedUri = "setting"
                                                             Label = "Term1B"
                                                             Selected = false
                                                             Children = []};



                                                ]};
                                         Term { Uri = Uri.from "http://testing.com/TestSetting2#Term2"
                                                ShortenedUri = "setting"
                                                Label = "Term2"
                                                Selected = false
                                                Children = []};
                                       ]};
              Property = "qualitystandard:setting"}]

let getSearchResults _ _ = [{Uri = "Uri1"; Abstract = "Unicorns under the age of 65..."; Title = "This is the title"};
                            {Uri = "Uri2"; Abstract = "Goblins with arthritis..."; Title = "This is the title"}]

let vocabsForTests = [{Root = Term {Uri = (Uri.from "http://testing.com/setting")
                                    ShortenedUri = "setting"
                                    Label = "Settings:"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/TestSetting1"
                                                        ShortenedUri = "setting"
                                                        Label = "Term1"
                                                        Selected = false
                                                        Children = []};
                                                 Term { Uri = Uri.from "http://testing.com/TestSetting2"
                                                        ShortenedUri = "setting"
                                                        Label = "Term2"
                                                        Selected = false
                                                        Children = []};]};
                       Property = "qualitystandard:setting"};
                      {Root = Term {Uri = (Uri.from "http://testing.com/ServiceArea")
                                    ShortenedUri = "setting"
                                    Label = "Service Area:"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://testing.com/TestArea1"
                                                        ShortenedUri = "setting"
                                                        Label = "Term3"
                                                        Selected = false
                                                        Children = []};
                                                 Term { Uri = Uri.from "http://testing.com/TestArea2"
                                                        ShortenedUri = "setting"
                                                        Label = "Term4"
                                                        Selected = false
                                                        Children = []};]};
                       Property = "qualitystandard:serviceArea"}]

let getKBCount _ = 0
