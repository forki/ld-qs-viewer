namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("viewer")>]
[<assembly: AssemblyProductAttribute("viewer")>]
[<assembly: AssemblyDescriptionAttribute("A linked data viewer for the knowledge base project")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
