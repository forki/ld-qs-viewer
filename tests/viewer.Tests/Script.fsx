#load "LoadDeps.fsx"

open Swensen.Unquote
open CsQuery

let x = CQ.Create("<input >")
x.Select("input[checked]").Length
