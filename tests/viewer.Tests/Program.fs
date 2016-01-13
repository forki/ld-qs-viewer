module Viewer.Tests.Program

open Fuchu
open Fuchu.Test
open Fuchu.Impl
open System
open System.Reflection

[<EntryPoint>]
let main args = 
  let printTestName name =
    printf "%s\n" name

  let printer = 
    { TestPrinters.Default with 
        BeforeRun = printTestName
        Failed = printFailed
        Exception = printException }

  printf "Running tests...\n"
  let res =
    match testFromAssembly (Assembly.GetEntryAssembly()) with
    | Some tests -> runEval (eval printer Seq.map) tests
    | None -> 0
  res
