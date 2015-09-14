#I "bin/Release"
#r "bin/Release/CsQuery.dll"
#r "bin/Release/DotLiquid.dll"
#r "bin/Release/Suave.dll"
#r "bin/Release/Suave.DotLiquid.dll"
#r "bin/Release/Suave.Testing.dll"
#r "bin/Release/viewer.dll"
#r "bin/Release/viewer.Tests.dll"

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

let GetVocabularies = []
let GetSearchResults = ""

let server = startServerWithData GetVocabularies GetSearchResults

// Example request
// let response = server |> req HttpMethod.GET "/" None |> ParseHtml;;
// Example request with querystring
// let response = server |> reqQuery HttpMethod.GET "/search" "q=1" |> ParseHtml;;
