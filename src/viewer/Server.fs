open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Viewer.App

[<EntryPoint>]
let main argv =
  //let defaultConfig = { defaultConfig with
  //                                      listenTimeout   = System.TimeSpan.FromMilliseconds 2000.
  //                                      bindings = [ HttpBinding.mk' HTTP "127.0.0.1" 8038 ]
  //                    }
  startWebServer defaultConfig app
  1
