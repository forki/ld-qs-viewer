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
open NUnit.Framework
open CsQuery
open Viewer.Tests

DotLiquid.setTemplatesDir("bin/Release/templates/")

let getAnnotations =
  [("Annotation 1", "http://ld.nice.org.uk/ns/Annotation_Uri1");
   ("Annotation 2", "http://ld.nice.org.uk/ns/Annotation_Uri2")]

let checkboxes =
  MakeRequest' HttpMethod.GET "/" getAnnotations
  |> (fun x -> x.Select("input"))

Assert.AreEqual(2, checkboxes.Length)
