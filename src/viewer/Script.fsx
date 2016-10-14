(* #r @"System.Net.Http"*)
(* #r "../../bin/viewer/viewer.dll"*)
(* #r "bin/Release/Unquote.dll"*)
(* open Viewer.SuaveExtensions*)
(* open Viewer.Components.Hotjar*)

(* setTemplatesDir "src/viewer/bin/Release/"*)


type TermD = 
  {
  Label : string
  Children : Term list
  } 
and Term =
  | Term of TermD
  | Empty  

let Term1 = Term {
            Label = "d"
            Children= []}

let terms = [Term {
              Label = "a"
              Children = [
                            Term {
                              Label = "b"
                              Children = [
                                         Term { Label = "c"; Children=[]} 
                                         Term { Label = "c"; Children=[]} 
                                         Term { Label = "c"; Children=[]} 
                              ]
                            };
                            Term {
                              Label = "bb"
                              Children = [
                                         Term { Label = "cc"; Children=[]} 
                                         Term { Label = "cc"; Children=[]} 
                                         Term { Label = "cc"; Children=[]} 
                              ]
                            };
                            
              ]
            };
            Term1;]


let rec loopChildren start children =
  let test x filters=
    Seq.exists (fun a -> a=x) filters

  List.fold (fun acc x -> match x with 
                          | Term x -> if (test x.Label ["a";"c"]) then
                                         acc
                                      else
                                         acc @ loopChildren [x.Label] x.Children
                          | Empty -> []) start children
    
loopChildren [] terms
(* let rec loopChildren start children =*)
(*   List.fold (fun acc x -> match x with *)
(*                           | Term x ->  acc @ loopChildren [x.Label] x.Children*)
(*                           | Empty -> []) start children*)
    
(* loopChildren [] terms*)


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



