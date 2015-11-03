#I "bin/Debug"
#r "bin/Debug/CsQuery.dll"
#r "bin/Debug/DotLiquid.dll"
#r "bin/Debug/Suave.dll"
#r "bin/Debug/Suave.DotLiquid.dll"
#r "bin/Debug/Suave.Testing.dll"
#r "bin/Debug/viewer.dll"
#r "bin/Debug/viewer.Tests.dll"

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open Viewer.Tests.Utils
open Viewer.App

setTemplatesDir "templates/"

let GetVocabularies () =

  let v = Viewer.VocabGeneration.GetVocabs ()
  printfn "%A" v
  v
let GetSearchResults x = []

let GetKBCount () =
  0

let server = startServerWithData GetVocabularies GetSearchResults GetKBCount

// Example request
// let response = server |> req HttpMethod.GET "/" None |> ParseHtml;;
// Example request with querystring
// let response = server |> reqQuery HttpMethod.GET "/search" "q=1" |> ParseHtml;;
