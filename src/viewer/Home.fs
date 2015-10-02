module Viewer.Home

open Suave
open Suave.Types
open Suave.Cookie
open Viewer.Types

let getTestVocabs () = [{Name = "setting"; 
                         Terms = [{Name = "Term1"; Uri = "http://testing.com/TestSetting1"};
                                  {Name = "Term2"; Uri = "http://testing.com/TestSetting2"}]};]

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
