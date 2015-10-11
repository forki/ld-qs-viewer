#load "LoadDeps.fsx"

open Swensen.Unquote
open Suave.Testing
open Viewer.Tests.Utils
open CsQuery
open Viewer.VocabGeneration
open FSharp.RDF


let vocabs = [{Root = Term {Uri = (Uri.from "http://testing.com/Vocab1")
                            Label = "Vocab 1"
                            Selected = false
                            Children = [
                                         Term { Uri = Uri.from "http://testing.com/Uri1"
                                                Label = "Term1"
                                                Selected = false
                                                Children = []};
                                         Term { Uri = Uri.from "http://testing.com/Uri2"
                                                Label = "Term2"
                                                Selected = false
                                                Children = []}]};
               Property = "v1"}]
let GetSearchResults _ _ = []

let html = startServerWithData vocabs GetSearchResults |> getQuery "/qs/search/" "key=http%3A%2F%2Ftesting.com%2FUri1"

let selectedCheckboxes = html |> CQ.select "input[checked]"

//test <@ selectedCheckboxes |> CQ.length = 1 @>
//test <@ selectedCheckboxes |> CQ.first |> CQ.attr "value" = "http://testing.com/Uri2" @>
