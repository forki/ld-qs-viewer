module Viewer.Home

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Types
open Viewer.VocabGeneration
open FSharp.RDF

let getTestVocabs () = [{Root = Term {Uri = (Uri.from "http://testing.com/setting")
                                      Label = "Settings:";
                                      Children = [
                                                   Term { Uri = Uri.from "http://testing.com/TestSetting1";
                                                          Label = "Term1";
                                                          Children = []};
                                                   Term { Uri = Uri.from "http://testing.com/TestSetting2";
                                                          Label = "Term2";
                                                          Children = []};]};
                        Property = "setting"}]

type HomeModel =  {
   Vocabularies: Vocabulary list
}
let home (req:HttpRequest) getVocabs =
  printf "Request: %A\n" req

  let testing = req.cookies |> Map.containsKey "test"
  let vocabs = 
    match testing with
    | true  -> getTestVocabs
    | false -> getVocabs

  DotLiquid.page "home.html" {Vocabularies = vocabs()}
