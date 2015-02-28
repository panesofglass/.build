#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open System
open System.IO
open System.Collections.Generic

// --------------------------------
// init.fsx 
// This file is run the first time that you run build.sh/build.cmd
// It generates the build.fsx and generate.fsx files 
// --------------------------------

// special funtions
// many whom might be replaceable with FAKE functions

let failfUnlessExists f msg p = if not <| File.Exists f then failwithf msg p
let combine p1 p2 = Path.Combine(p2, p1)
let move p1 p2 = 
  if File.Exists p1 then
    printfn "moving %s to %s" p1 p2
    File.Move(p1, p2)
  elif Directory.Exists p1 then
    printfn "moving directory %s to %s" p1 p2
    Directory.Move(p1, p2)
  else
    failwithf "Could not move %s to %s" p1 p2
let localFile f = combine f __SOURCE_DIRECTORY__ 
let buildTemplatePath = localFile "build.template" 
let outputPath = localFile "build.fsx" 

let prompt (msg:string) = 
  Console.Write(msg)
  Console.ReadLine().Trim() 
  |> function | "" -> None | s -> Some s
  |> Option.map (fun s -> s.Replace ("\"","\\\""))
let runningOnAppveyor =
  not <| String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"))
let runningOnTravis =
  not <| String.IsNullOrEmpty(Environment.GetEnvironmentVariable("TRAVIS"))
let inCI = runningOnAppveyor || runningOnTravis
let promptFor friendlyName = 
  if inCI then Some "CONTINUOUSINTEGRATION"
  else prompt (sprintf "%s: " friendlyName)
let rec promptForNoSpaces friendlyName =
  match promptFor friendlyName with
  | None -> None
  | Some s when not <| String.exists (fun c -> c = ' ') s -> Some s
  | _ -> Console.WriteLine("Sorry, spaces are disallowed"); promptForNoSpaces friendlyName

failfUnlessExists buildTemplatePath "Cannot find build template file %s" 
  (Path.GetFullPath buildTemplatePath)

// User input
let border = "#####################################################"
let print msg = 
  printfn """
  %s
  %s
  %s
  """ border msg border

print """
# Project Scaffold Init Script
# Please answer a few questions and we will generate
# two files:
#
# build.fsx               This will be your build script
# docs/tools/generate.fsx This script will generate your 
#                         documentation
#
# NOTE: Aside from the Project Name, you may leave any 
# of these blank, but you will need to change the defaults 
# in the generated scripts.
#
"""

let vars = Dictionary<string,string option>()
vars.["##ProjectName##"] <- promptForNoSpaces "Project Name (used for solution/project files)"
vars.["##Summary##"]     <- promptFor "Summary (a short description)"
vars.["##Description##"] <- promptFor "Description (longer description used by NuGet)"
vars.["##Author##"]      <- promptFor "Author"
vars.["##Tags##"]        <- promptFor "Tags (separated by spaces)"
vars.["##GitHome##"]     <- promptFor "Github User or Organization"
vars.["##GitName##"]     <- promptFor "Github Project Name (leave blank to use Project Name)"

//Now that everything is renamed, we need to update the content of some files
let replace t r (lines:seq<string>) = 
  seq { 
    for s in lines do 
      if s.Contains(t) then yield s.Replace(t, r) 
      else yield s }

let replaceWithVarOrMsg t n lines = 
    replace t (vars.[t] |> function | None -> n | Some s -> s) lines

//Replace tokens in build template
let generate templatePath generatedFilePath = 
  failfUnlessExists templatePath "Cannot find template %s" (templatePath |> Path.GetFullPath)
  
  let newContent = 
    File.ReadAllLines(templatePath) |> Array.toSeq 
    |> replace "##ProjectName##" projectName
    |> replaceWithVarOrMsg "##Summary##" "Project has no summmary; update build.fsx"
    |> replaceWithVarOrMsg "##Description##" "Project has no description; update build.fsx"
    |> replaceWithVarOrMsg "##Author##" "Update Author in build.fsx" 
    |> replaceWithVarOrMsg "##Tags##" "" 
    |> replaceWithVarOrMsg "##GitHome##" "Update GitHome in build.fsx"
    |> replace "##GitName##" projectName

  File.WriteAllLines(generatedFilePath, newContent)
  File.Delete(templatePath)
  print (sprintf "# Generated %s" generatedFilePath)

generate (localFile "build.template") (localFile "build.fsx")
File.Delete "init.fsx"

