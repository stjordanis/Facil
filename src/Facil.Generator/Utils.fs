﻿[<AutoOpen>]
module internal Facil.Utils


open System


exception FacilException of printMsg: (unit -> unit)

// Format:
// https://docs.microsoft.com/en-us/cpp/build/formatting-the-output-of-a-custom-build-step-or-build-event?view=msvc-160
// Potentially also relevent:
// https://docs.microsoft.com/en-us/visualstudio/msbuild/exec-task?view=vs-2019

let private log warnOrErr msg =
  Console.WriteLine($"Facil : %s{warnOrErr} : %s{msg}")


let private logYaml fullPath lineNo colNo warnOrErr msg =
  Console.WriteLine($"%s{fullPath}(%i{lineNo} , %i{colNo}) : %s{warnOrErr} : %s{msg}")

let logWarning = log "warning"

let logError = log "error"

let logYamlWarning fullPath lineNo colNo msg = logYaml fullPath lineNo colNo "warning" msg

let logYamlError fullPath lineNo colNo msg = logYaml fullPath lineNo colNo "error" msg

let failwithError<'a> msg : 'a = raise (FacilException (fun () -> logError msg))

let failwithYamlError<'a> fullPath lineNo colNo msg : 'a =
  raise (FacilException (fun () -> logYamlError fullPath lineNo colNo msg))



module String =

  let firstUpper (s: string) =
    (s |> Seq.head).ToString().ToUpperInvariant() + s.Substring(1)

  let firstLower (s: string) =
    (s |> Seq.head).ToString().ToLowerInvariant() + s.Substring(1)

  let trimStart (trim: char) (s: string) =
    s.TrimStart(trim)

  let getDeindentedLines (s: string) =
    let s =
      s.Split('\n')
      |> Array.toList
      |> List.map (fun s -> s.TrimEnd('\r').Replace("\t", "  "))
    let minIndent =
      s
      |> List.map (fun s -> s |> Seq.takeWhile ((=) ' ') |> Seq.length)
      |> List.min
    s |> List.map (fun s -> s.Substring(minIndent))



module Map =

  let merge mergeItem initialMap mergeMap =
    let folder outMap key item =
      match outMap |> Map.tryFind key with
      | None -> outMap.Add(key, item)
      | Some existingItem -> outMap.Add(key, mergeItem existingItem item)
    Map.fold folder initialMap mergeMap

  let mergeWithNoneKeyInheritance mergeItem (initialMap: Map<_,_>) (mergeMap: Map<_,_>) =
    let paramsToMerge =
      match mergeMap.TryFind None, initialMap.TryFind None with
      | None, None -> mergeMap
      | Some baseParam, None | None, Some baseParam ->
          mergeMap |> Map.map (fun _ param -> mergeItem baseParam param)
      | Some baseParam1, Some baseParam2 ->
          let baseParam = mergeItem baseParam1 baseParam2
          mergeMap |> Map.map (fun _ param -> mergeItem baseParam param)
    merge mergeItem initialMap paramsToMerge



module List =

  let mapAllExceptLast f list =
    let length = List.length list
    list |> List.mapi (fun i x -> if i <> length - 1 then f x else x)



module Option =

  let teeNone f (opt: 'a option) =
    if opt.IsNone then f ()
    opt
