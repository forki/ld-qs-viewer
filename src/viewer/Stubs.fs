module Stubs
open Viewer.Types
open Viewer.Data.Vocabs.VocabGeneration
open FSharp.RDF


let vocabs = [{Root = Term {Uri = (Uri.from "http://ld.nice.org.uk/ns/qualitystandard/setting")
                            ShortenedUri = "setting"
                            Label = "Settings:"
                            Selected = false
                            Children = [
                                         Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting1#Term1"
                                                ShortenedUri = "TestSetting1#Term1"
                                                Label = "Term1"
                                                Selected = false
                                                Children = [
                                                             Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting1#Term1A"
                                                                    ShortenedUri = "TestSetting1#Term1A"
                                                                    Label = "Term1A"
                                                                    Selected = false
                                                                    Children = [
                                                                                 Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting1#Term1AA"
                                                                                        ShortenedUri = "TestSetting1#Term1AA"
                                                                                        Label = "Term1AA"
                                                                                        Selected = false
                                                                                        Children = []};
                                                                   ]};
                                                      Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting1#Term1B"
                                                             ShortenedUri = "setting"
                                                             Label = "Term1B"
                                                             Selected = false
                                                             Children = []};



                                                ]};
                                         Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting2#Term2"
                                                ShortenedUri = "setting"
                                                Label = "Term2"
                                                Selected = false
                                                Children = []};
                                       ]};
              Property = "qualitystandard:setting";
               Label = "Setting"}]

let getSearchResults _ _ = [{Uri = "Uri1"; Abstract = "Unicorns under the age of 65..."; Title = "This is the title"};
                            {Uri = "Uri2"; Abstract = "Goblins with arthritis..."; Title = "This is the title"}]

let vocabsForTests = [{Root = Term {Uri = (Uri.from "http://ld.nice.org.uk/ns/qualitystandard/setting")
                                    ShortenedUri = "setting"
                                    Label = "Settings:"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting#Term1"
                                                        ShortenedUri = "TestSetting#Term1"
                                                        Label = "Term1"
                                                        Selected = false
                                                        Children = []};
                                                 Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestSetting#Term2"
                                                        ShortenedUri = "TestSetting#Term2"
                                                        Label = "Term2"
                                                        Selected = false
                                                        Children = []};]};
                       Property = "qualitystandard:setting";
                       Label = "setting"};
                      {Root = Term {Uri = (Uri.from "http://ld.nice.org.uk/ns/qualitystandard/ServiceArea")
                                    ShortenedUri = "setting"
                                    Label = "Service Area:"
                                    Selected = false
                                    Children = [
                                                 Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestArea#Term1"
                                                        ShortenedUri = "TestArea#Term1"
                                                        Label = "Term1"
                                                        Selected = false
                                                        Children = []};
                                                 Term { Uri = Uri.from "http://ld.nice.org.uk/ns/qualitystandard/TestArea#Term2"
                                                        ShortenedUri = "TestArea#Term2"
                                                        Label = "Term2"
                                                        Selected = false
                                                        Children = []};]};
                       Property = "qualitystandard:serviceArea";
                       Label = "serviceArea"}]

let getKBCount _ = 0
