module Viewer.Home

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Types

let getTestVocabs () = [{Label = "Settings:";
                         Name = "setting";
                         Terms = [{Name = "Term1"; Uri = "http://testing.com/TestSetting1"; Selected = false};
                                  {Name = "Term2"; Uri = "http://testing.com/TestSetting2"; Selected = false}]};]

type HomeModel =  {
   Vocabularies: Vocabulary list
   totalCount: int
}
let home (req:HttpRequest) getVocabs =
  printf "Request: %A\n" req

  let testing = req.cookies |> Map.containsKey "test"
  let vocabs = 
    match testing with
    | true  -> getTestVocabs
    | false -> getVocabs

  DotLiquid.page "home.html" {Vocabularies = vocabs(); totalCount = 0}
