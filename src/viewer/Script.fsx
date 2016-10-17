(* #r @"System.Net.Http"*)
(* #r "../../bin/viewer/viewer.dll"*)
(* #r "bin/Release/Unquote.dll"*)
(* open Viewer.SuaveExtensions*)
(* open Viewer.Components.Hotjar*)

(* setTemplatesDir "src/viewer/bin/Release/"*)


type TermD = 
  {
  Label : string
  Property : string
  Children : Term list
  } 
and Term =
  | Term of TermD
  | Empty  

let Term1 = Term {
            Label = "d"
            Property = "Property D"
            Children= []}

let terms = [Term {
              Label = "a"
              Property = "Property A"
              Children = [
                            Term {
                              Label = "b"
                              Property = "Property B"
                              Children = [
                                         Term { Label = "c"; Property = "C"; Children=[]} 
                                         Term { Label = "c"; Property = "C"; Children=[]} 
                                         Term { Label = "c"; Property = "C"; Children=[]} 
                              ]
                            };
                            Term {
                              Label = "bb"
                              Property = "BB"; 
                              Children = [
                                         Term { Label = "cc"; Property = "CC"; Children=[]} 
                                         Term { Label = "cc"; Property = "CC"; Children=[]} 
                                         Term { Label = "cc"; Property = "CC"; Children=[]} 
                              ]
                            };
                            
              ]
            };
            Term1;]

(* return each item specified in the filter *)
    
let getItems searchByProperty getProperty searchTerms =
  let searchFn filters searchByProperty x =
    Seq.exists (fun a ->a = searchByProperty x) filters

  let search currentTerm =
   searchFn searchTerms searchByProperty currentTerm
  
  let rec recurseTree start children =
    List.fold (fun acc term ->  match term with
                                | Term term -> if (search term) then
                                                  [getProperty term] @ acc @ recurseTree start term.Children
                                                else
                                                  acc @ recurseTree start term.Children
                                | Empty -> []) start children

  recurseTree [] terms

let propertyToSearchFor term = term.Property 
let propertyToRetrieve term = term.Property 

getItems propertyToSearchFor propertyToRetrieve ["a";"c"]

(* basic recursion *)

(* let rec loopChildren start children  f =*)
(*   let test x filters=*)
(*     Seq.exists (fun a->a=x) filters*)

(*   List.fold (fun acc x -> match x with *)
(*                           | Term x -> if (f x) then*)
(*                                         [x.Label] @ acc @ loopChildren start x.Children f*)
(*                                       else*)
(*                                         acc @ loopChildren start x.Children f*)
(*                           | Empty -> []) start children*)

(* Return each element as it is *)
(* let rec loopChildren start children =*)
(*   List.fold (fun acc x -> match x with*)
(*                           | Term x ->  acc @ loopChildren [x.Label] x.Children*)
(*                           | Empty -> []) start children*)


let a = [1;2;3;4;]
let b = ["one"; "two"; "three"; "four";]
List.fold (fun acc x->
                match x with
                | x when x % 2 =0 -> x + acc
                | _ -> 0) 0 a

List.fold (fun acc x -> acc + "qs=" + x + "&") "?" b

type temp = {
  Name: string
  Fields: string list
}

let expected = [{Name = "Section1"; Fields = ["Field"]}
                {Name = "Section2"; Fields = ["Field"]}]


List.fold (fun acc x -> acc + x.Name; x.Fields @ acc) "" expected



