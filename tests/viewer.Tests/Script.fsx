#load "LoadDeps.fsx" 
open CsQuery


let cs = CQ ("<div>yooho</div>")


type Configuration = {
  Vocabs : Vocabulary list
  GetSearchResults : (bool -> string -> SearchResult list)
  GetKBCount : bool -> int
  }

