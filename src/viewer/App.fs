module Viewer.App

open Suave
open Suave.Web
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Files
open Suave.Http.Successful
open Suave.Types
open Suave.Cookie
open Suave.Log
open Suave.Utils
open Viewer.Types
open Viewer.Search
open Viewer.Home
open FSharp.Data

let setTemplatesDir path =
  DotLiquid.setTemplatesDir(path)

let qualityStandardsDir = "/artifacts/published/"

let createApp getVocabs getSearchResults =
  choose
    [ GET >>= choose
        [path "/" >>= request(fun req -> home req getVocabs)
         path "/search" >>= request(fun req -> search req getSearchResults getVocabs)
         path "/testing" >>= request(fun req ->
                                     printf "request: %A\n" req
                                     printf "headers: %s\n" (req.headers.ToString())
                                     printf "cookies: %s\n" (req.cookies.ToString())
                                     OK "boo")
         browse qualityStandardsDir
         browseHome
         RequestErrors.NOT_FOUND "Found no handlers"]]


