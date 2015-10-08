#load "LoadDeps.fsx"

open Swensen.Unquote
open CsQuery

let x = CQ.Create("<input type='checkbox'>")
x.Select("input")
