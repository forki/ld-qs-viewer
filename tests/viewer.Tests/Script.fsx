#I "bin/Release"
#r "bin/Release/CsQuery.dll"
#r "bin/Release/DotLiquid.dll"
#r "bin/Release/FsPickler.dll"
#r "bin/Release/Fuchu.dll"
#r "bin/Release/nunit.framework.dll"
#r "bin/Release/Suave.dll"
#r "bin/Release/Suave.DotLiquid.dll"
#r "bin/Release/Suave.Testing.dll"
#r "bin/Release/viewer.Tests.dll"

open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open CsQuery
open Viewer.Tests
open Suave.DotLiquid

DotLiquid.setTemplatesDir("bin/Release/templates/")

let html = CQ ("""
               <div>Div1
                 <div class="vocab">Vocab1
                   <input/>
                   <input/>
                 </div
                 <div class="vocab">Vocab2
                   <input/>
                 </div
               </div>
               """)


