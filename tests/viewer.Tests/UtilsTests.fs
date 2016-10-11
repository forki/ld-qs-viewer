module Viewer.Tests.UtilsTests

open NUnit.Framework
open Viewer.Utils
open Viewer.Types
open FsUnit
open FSharp.RDF


let private vocabs = [{Root = Term {
                            Uri = Uri.from "http://testing.com/Uri3"
                            ShortenedUri = "unknown"
                            Label = "Care home"
                            Selected = false
                            Children = [
                                         Term { 
                                                Uri = Uri.from "http://testing.com/Uri3"
                                                ShortenedUri = "vocabLabel/long-guid-1"
                                                Label = "Term1"
                                                Selected = false
                                                Children = []};
                                         Term { 
                                                Uri = Uri.from "http://testing.com/Uri3"
                                                ShortenedUri = "vocabLabel/long-guid-2"
                                                Label = "Term2"
                                                Selected = false
                                                Children = []};
                                         Term { 
                                                Uri = Uri.from "http://testing.com/Uri3"
                                                ShortenedUri = "vocabLabel/long-guid-3"
                                                Label = "Term3"
                                                Selected = false
                                                Children = []}]};
               Property = "v1";
               Label = ""}]

    
[<Test>]
let ``extractFilters should return empty list given an empty querystring`` () =
  let qs = []
  let filters = extractFilters qs
  filters |> should equal []

    
[<Test>]
let ``extractFilters should convert query string pairs into record type`` () =
  let qs = [("vocab1", Some("uri1"));
            ("vocab2", Some("uri2"))]
  let filters = extractFilters qs

  filters |> should equal [{Vocab = "vocab1"; TermUri = "uri1"}
                           {Vocab = "vocab2"; TermUri = "uri2"}]

    
[<Test>]
let ``aggregateQueryStringValues should group by keys`` () =
  let qs = [
      {Vocab="key1"; TermUri="val1"}
      {Vocab="key1"; TermUri="val2"}
      {Vocab="key2"; TermUri="val3"}
      ] 
  let aggs = aggregateFiltersByVocab qs
  aggs |> should equal [{Vocab="key1";TermUris=["val1";"val2"]}
                        {Vocab="key2";TermUris=["val3"]}]

    
[<Test>]
let ``createFilterTags should create filter tags from filters`` () =
 
  let filters = [{Vocab = "vocab"; TermUri = "vocabLabel/long-guid-2"}
                 {Vocab = "vocab"; TermUri = "vocabLabel/long-guid-1"}
                ]
  let filterTags = createFilterTags filters vocabs
  filterTags |> should equal [{Label = "Term2"; RemovalQueryString = "vocab=vocabLabel%2Flong-guid-1"}
                              {Label = "Term1"; RemovalQueryString = "vocab=vocabLabel%2Flong-guid-2"}
                             ] 


[<Category("RunOnly")>]
[<Test>]
let ``Should return an empty array when labels are not found`` () =
  let vocabs = [{Root = Term {
                              Uri = Uri.from "http://testing.com/Uri3"
                              ShortenedUri = "unknown"
                              Label = "Care home"
                              Selected = false
                              Children = [
                                           Term { 
                                                  Uri = Uri.from "http://testing.com/Uri3"
                                                  ShortenedUri = "vocabLabel/long-guid-1"
                                                  Label = "Term1"
                                                  Selected = false
                                                  Children = []};
                                           Term { 
                                                  Uri = Uri.from "http://testing.com/Uri3"
                                                  ShortenedUri = "vocabLabel/long-guid-2"
                                                  Label = "Term2"
                                                  Selected = false
                                                  Children = []};
                                           Term { 
                                                  Uri = Uri.from "http://testing.com/Uri3"
                                                  ShortenedUri = "vocabLabel/long-guid-3"
                                                  Label = "Term3"
                                                  Selected = false
                                                  Children = []}]};
                 Property = "v1";
                 Label = ""}]
    
  let filters = [ 
                {Vocab = "notused"; TermUri = "guid-does-not-exist-1"};
                {Vocab = "notused"; TermUri = "guid-does-not-exist-2"}
                ]

  let filterTags = createFilterTags filters vocabs
  filterTags |> should equal [] 

[<Category("RunOnly")>]
[<Test>]
let ``Should return guid when label given`` () =

  let vocabs = [{Root = Term {
                              Uri = Uri.from "http://testing.com/agegroup"
                              ShortenedUri = "unknown/unknown"
                              Label = "Age group"
                              Selected = false
                              Children = [
                                           Term { 
                                                  Uri = Uri.from "http://testing.com/qualitystandard/agegroup/aa4da4d7_b934_4d03_b556_f7b97381953f"
                                                  ShortenedUri = "agegroup/aa4da4d7_b934_4d03_b556_f7b97381953f"
                                                  Label = "All age groups"
                                                  Selected = false
                                                  Children = [
                                                              Term {
                                                                     Uri = Uri.from "http://testing.com/qualitystandard/agegroup/d3326f46_c734_4ab7_9e41_923256bd7d0b"
                                                                     ShortenedUri = "agegroup/d3326f46_c734_4ab7_9e41_923256bd7d0b"
                                                                     Label = "Adults"
                                                                     Selected = false
                                                                     Children = [
                                                                                 Term {
                                                                                         Uri = Uri.from "http://testing.com/qualitystandard/agegroup/c4347520_adf4_4ddb_9926_8f6c3132525e"
                                                                                         ShortenedUri = "agegroup/c4347520_adf4_4ddb_9926_8f6c3132525e"
                                                                                         Label = "Adults 18-24 years"
                                                                                         Selected = false
                                                                                         Children = []
                                                                                 }
                                                                                 Term {
                                                                                         Uri = Uri.from "http://testing.com/qualitystandard/agegroup/c7935d78_d1ad_47f3_98a6_f0af04956b97"
                                                                                         ShortenedUri = "agegroup/c7935d78_d1ad_47f3_98a6_f0af04956b97"
                                                                                         Label = "Adults 25-64 years"
                                                                                         Selected = false
                                                                                         Children = []
                                                                                 }
                                                                     ]
                                                              }
                                                              Term {
                                                                     Uri = Uri.from "http://testing.com/qualitystandard/agegroup/35a5e24c_4e78_40ff_8bef_202ce406e25a"
                                                                     ShortenedUri = "agegroup/35a5e24c_4e78_40ff_8bef_202ce406e25a"
                                                                     Label = "Children"
                                                                     Selected = false
                                                                     Children = [
                                                                                 Term {
                                                                                         Uri = Uri.from "http://testing.com/qualitystandard/agegroup/298765b2_34e6_4f0b_ba25_f0fa84e2a25e"
                                                                                         ShortenedUri = "agegroup/agegroup/298765b2_34e6_4f0b_ba25_f0fa84e2a25e"
                                                                                         Label = "Babies 0-1 year"
                                                                                         Selected = false
                                                                                         Children = [
                                                                                                     Term {
                                                                                                             Uri = Uri.from "http://testing.com/qualitystandard/agegroup/1b926ed3_9f9c_406d_8cc0_68e439172fea"
                                                                                                             ShortenedUri = "agegroup/agegroup/1b926ed3_9f9c_406d_8cc0_68e439172fea"
                                                                                                             Label = "Babies 1 month-1 year"
                                                                                                             Selected = false
                                                                                                             Children = []
                                                                                                     }
                                                                                                     Term {
                                                                                                             Uri = Uri.from "http://testing.com/qualitystandard/agegroup/d9698e00_3dd4_4273_96df_9f1be216cb89"
                                                                                                             ShortenedUri = "agegroup/d9698e00_3dd4_4273_96df_9f1be216cb89"
                                                                                                             Label = "Neonates 0-28 days"
                                                                                                             Selected = false
                                                                                                             Children = []
                                                                                                     }
                                                                                         ]
                                                                                 }
                                                                                 Term {
                                                                                         Uri = Uri.from "http://testing.com/qualitystandard/agegroup/5d69430f_dace_4c7e_bc70_70b02cd3d965"
                                                                                         ShortenedUri = "agegroup/5d69430f_dace_4c7e_bc70_70b02cd3d965"
                                                                                         Label = "Children 1-15 years"
                                                                                         Selected = false
                                                                                         Children = []
                                                                                 }
                                                                     ]
                                                              }
                                                  ]};
                                           ]};
                 Property = "v1";
                 Label = ""}]
    

  getGuids [//"All age groups"
            //"Adults"
            //"Adults 18-24 years"
            "Children 1-15 years"] vocabs
  |> should equal [//"aa4da4d7_b934_4d03_b556_f7b97381953f"
                  // "d3326f46_c734_4ab7_9e41_923256bd7d0b"
                  // "c4347520_adf4_4ddb_9926_8f6c3132525e"
                   "5d69430f_dace_4c7e_bc70_70b02cd3d965" ] 


[<Test>]
let ``Should return empty array when label not found`` () =
    
  let labels = [ "Label not found 1"; "Label not found 2" ]

  let guids = getGuids labels vocabs
  guids |> should equal [] 

