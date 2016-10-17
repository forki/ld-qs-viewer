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
            Property = "Property D/PropD"
            Children= []}

let terms = [Term {
              Label = "a"
              Property = "Property A/PropA"
              Children = [
                            Term {
                              Label = "b"
                              Property = "Property B/PropB"
                              Children = [
                                         Term { Label = "c"; Property = "C/PropC"; Children=[]} 
                                         Term { Label = "c"; Property = "C/PropC"; Children=[]} 
                                         Term { Label = "c"; Property = "C/PropC"; Children=[]} 
                              ]
                            };
                            Term {
                              Label = "bb"
                              Property = "BB/PropBB"; 
                              Children = [
                                         Term { Label = "cc"; Property = "CC/PropCC"; Children=[]} 
                                         Term { Label = "cc"; Property = "CC/PropCC"; Children=[]} 
                                         Term { Label = "cc"; Property = "CC/PropCC"; Children=[]} 
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

let propertyToSearchFor term = term.Property.Split('/').[1] 
let propertyToRetrieve term = term.Label 

getItems propertyToSearchFor propertyToRetrieve ["PropA";"PropC"]

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



