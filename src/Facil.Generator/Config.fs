﻿[<AutoOpen>]
module Facil.Config

open System.IO
open System.Text.RegularExpressions
open GlobExpressions


type IncludeOrFor =
  | Include of pattern: string
  | For of pattern: string


type ResultKind =
  | Auto
  | AnonymousRecord
  | NominalRecord
  | Custom of string


[<CLIMutable>]
type ConfigSourceDto = {
  appSettings: string option
  userSecrets: string option
  envVars: string option
}


[<CLIMutable>]
type TableDtoColumnDto = {
  skip: bool option
}


type TableDtoColumn = {
  Skip: bool option
}


[<CLIMutable>]
type TableDtoRuleDto = {
  ``include``: string option
  ``for``: string option
  except: string option
  voption: bool option
  columns: Map<string, TableDtoColumnDto> option
}


type TableDtoRule = {
  IncludeOrFor: IncludeOrFor
  Except: string option
  Voption: bool option
  Columns: Map<string option, TableDtoColumn>
}


type EffectiveTableDtoRule = {
  Voption: bool
  ColumnsFromAllRules: Map<string option, TableDtoColumn> list
}

[<CLIMutable>]
type TableTypeRuleDto = {
  ``for``: string option
  except: string option
  voption: bool option
  skipParamDto: bool option
}


type TableTypeRule = {
  For: string
  Except: string option
  Voption: bool option
  SkipParamDto: bool option
}


type EffectiveTableTypeRule = {
  Voption: bool
  SkipParamDto: bool
}


[<CLIMutable>]
type ProcedureParameterDto = {
  dtoName: string option
  buildValue: string option
}


type ProcedureParameter = {
  DtoName: string option
  BuildValue: string option
}


[<CLIMutable>]
type ProcedureColumnDto = {
  skip: bool option
}


type ProcedureColumn = {
  Skip: bool option
}


[<CLIMutable>]
type ProcedureRuleDto = {
  ``include``: string option
  ``for``: string option
  except: string option
  result: string option
  voptionIn: bool option
  voptionOut: bool option
  recordIfSingleCol: bool option
  skipParamDto: bool option
  useReturnValue: bool option
  ``params``: Map<string, ProcedureParameterDto> option
  columns: Map<string, ProcedureColumnDto> option
}


type ProcedureRule = {
  IncludeOrFor: IncludeOrFor
  Except: string option
  Result: ResultKind option
  VoptionIn: bool option
  VoptionOut: bool option
  RecordIfSingleCol: bool option
  SkipParamDto: bool option
  UseReturnValue: bool option
  Parameters: Map<string option, ProcedureParameter>
  Columns: Map<string option, ProcedureColumn>
}


type EffectiveProcedureRule = {
  Result: ResultKind
  VoptionIn: bool
  VoptionOut: bool
  RecordIfSingleCol: bool
  SkipParamDto: bool
  UseReturnValue: bool
  ParametersFromAllRules: Map<string option, ProcedureParameter> list
  ColumnsFromAllRules: Map<string option, ProcedureColumn> list
}


[<CLIMutable>]
type ScriptParameterDto = {
  nullable: bool option
  ``type``: string option
  dtoName: string option
  buildValue: string option
}


type ScriptParameter = {
  Nullable: bool option
  Type: string option
  DtoName: string option
  BuildValue: string option
}

[<CLIMutable>]
type ScriptTempTableRuleDto = {
  definition: string
}


type ScriptTempTableRule = {
  Definition: string
}


[<CLIMutable>]
type ScriptColumnDto = {
  skip: bool option
}


type ScriptColumn = {
  Skip: bool option
}


[<CLIMutable>]
type ScriptRuleDto = {
  ``include``: string option
  ``for``: string option
  except: string option
  result: string option
  voptionIn: bool option
  voptionOut: bool option
  recordIfSingleCol: bool option
  skipParamDto: bool option
  ``params``: Map<string, ScriptParameterDto> option
  tempTables: ScriptTempTableRuleDto list option
  columns: Map<string, ScriptColumnDto> option
}


type ScriptRule = {
  IncludeOrFor: IncludeOrFor
  Except: string option
  IncludeMatches: Set<string>
  ForMatches: Set<string>
  Result: ResultKind option
  VoptionIn: bool option
  VoptionOut: bool option
  RecordIfSingleCol: bool option
  SkipParamDto: bool option
  Parameters: Map<string option, ScriptParameter>
  TempTables: ScriptTempTableRule list option
  Columns: Map<string option, ScriptColumn>
}


type EffectiveScriptRule = {
  Result: ResultKind
  VoptionIn: bool
  VoptionOut: bool
  RecordIfSingleCol: bool
  SkipParamDto: bool
  ParametersFromAllRules: Map<string option, ScriptParameter> list
  TempTables: ScriptTempTableRule list
  ColumnsFromAllRules: Map<string option, ScriptColumn> list
}


type ProcedureOrScriptParameter = {
  DtoName: string option
  BuildValue: string option
}


type ProcedureOrScriptColumn = {
  Skip: bool option
}


type EffectiveProcedureOrScriptRule = {
  Result: ResultKind
  VoptionIn: bool
  VoptionOut: bool
  RecordIfSingleCol: bool
  SkipParamDto: bool
  UseReturnValue: bool
  ParametersFromAllRules: Map<string option, ProcedureOrScriptParameter> list
  ColumnsFromAllRules: Map<string option, ProcedureOrScriptColumn> list
}


[<CLIMutable>]
type RuleSetDto = {
  connectionString: string option
  filename: string option
  namespaceOrModuleDeclaration: string option
  scriptBasePath: string option
  prelude: string option
  tableDtos: TableDtoRuleDto list option
  tableTypes: TableTypeRuleDto list option
  procedures: ProcedureRuleDto list option
  scripts: ScriptRuleDto list option
}


type RuleSet = {
  ConnectionString: Lazy<string>
  Filename: string
  NamespaceOrModuleDeclaration: string
  ScriptBasePath: string
  Prelude: string list option
  TableDtos: TableDtoRule list
  TableTypes: TableTypeRule list
  Procedures: ProcedureRule list
  Scripts: ScriptRule list
}


[<CLIMutable>]
type FacilConfigDto = {
  configs: ConfigSourceDto list option
  rulesets: RuleSetDto list option
}


module TableDtoColumn =


  let empty : TableDtoColumn =
    {
      Skip = None
    }


  let fromDto (dto: TableDtoColumnDto) : TableDtoColumn =
    {
      Skip = dto.skip
    }


  let merge (c1: TableDtoColumn) (c2: TableDtoColumn) : TableDtoColumn = {
    Skip = c2.Skip |> Option.orElse c1.Skip
  }


module TableDtoRule =


  let defaultEffectiveRule : EffectiveTableDtoRule = {
    Voption = false
    ColumnsFromAllRules = []
  }


  let fromDto fullYamlPath (dto: TableDtoRuleDto) : TableDtoRule = {
    IncludeOrFor =
      match dto.``include``, dto.``for`` with
      | None, None -> failwithYamlError fullYamlPath 0 0 "Either 'include' or 'for' is required in a tableDto rule"
      | Some inc, None -> Include inc
      | None, Some f -> For f
      | Some _, Some _ -> failwithYamlError fullYamlPath 0 0 "'include' and 'for' may not be combined in a tableDto rule"
    Except = dto.except
    Voption = dto.voption
    Columns =
      dto.columns
      |> Option.defaultValue Map.empty
      |> Map.toList
      |> List.map (fun (k, v) -> if k = "" then None, v else Some k, v)
      |> Map.ofList
      |> Map.map (fun _ -> TableDtoColumn.fromDto)
  }


  let matches schemaName tableName (rule: TableDtoRule) =
    let qualifiedName = $"%s{schemaName}.%s{tableName}"
    let pattern =
      match rule.IncludeOrFor with
      | For x -> x
      | Include x -> x
    match rule.Except with
    | None -> Regex.IsMatch(qualifiedName, pattern)
    | Some exPattern -> Regex.IsMatch(qualifiedName, pattern) && not <| Regex.IsMatch(qualifiedName, exPattern)


  let merge (eff: EffectiveTableDtoRule) (rule: TableDtoRule) : EffectiveTableDtoRule =
    {
      Voption = rule.Voption |> Option.defaultValue eff.Voption
      ColumnsFromAllRules = eff.ColumnsFromAllRules @ [rule.Columns]
    }


module EffectiveTableDtoRule =


  let getColumn colName (rule: EffectiveTableDtoRule) =
    rule.ColumnsFromAllRules
    |> List.collect (fun map ->
        [
          yield! map.TryFind None |> Option.toList
          yield! map.TryFind (Some colName) |> Option.toList
        ]
    )
    |> List.fold TableDtoColumn.merge TableDtoColumn.empty


  let allColumnNames (rule: EffectiveTableDtoRule) =
    rule.ColumnsFromAllRules
    |> List.collect (Map.toList >> List.map fst >> List.choose id)
    |> set


module TableTypeRule =


  let defaultEffectiveRule : EffectiveTableTypeRule = {
    Voption = false
    SkipParamDto = false
  }


  let fromDto fullYamlPath (dto: TableTypeRuleDto) : TableTypeRule = {
    For =
      dto.``for``
      |> Option.defaultWith (fun () -> failwithYamlError fullYamlPath 0 0 "Either 'include' or 'for' is required in a tableType rule")
    Except = dto.except
    Voption = dto.voption
    SkipParamDto = dto.skipParamDto
  }


  let matches schemaName tableName (rule: TableTypeRule) =
    let qualifiedName = $"%s{schemaName}.%s{tableName}"
    match rule.Except with
    | None -> Regex.IsMatch(qualifiedName, rule.For)
    | Some exPattern -> Regex.IsMatch(qualifiedName, rule.For) && not <| Regex.IsMatch(qualifiedName, exPattern)


  let merge (eff: EffectiveTableTypeRule) (rule: TableTypeRule) : EffectiveTableTypeRule =
    {
      Voption = rule.Voption |> Option.defaultValue eff.Voption
      SkipParamDto = rule.SkipParamDto |> Option.defaultValue eff.SkipParamDto
    }


module ProcedureParameter =


  let empty : ProcedureParameter = {
    DtoName = None
    BuildValue = None
  }


  let fromDto (dto: ProcedureParameterDto) : ProcedureParameter = {
    DtoName = dto.dtoName
    BuildValue = dto.buildValue
  }


  let merge (p1: ProcedureParameter) (p2: ProcedureParameter) : ProcedureParameter = {
    DtoName = p2.DtoName |> Option.orElse p1.DtoName
    BuildValue = p2.BuildValue |> Option.orElse p1.BuildValue
  }


module ProcedureColumn =


  let empty : ProcedureColumn = {
    Skip = None
  }


  let fromDto (dto: ProcedureColumnDto) : ProcedureColumn =
    {
      Skip = dto.skip
    }


  let merge (c1: ProcedureColumn) (c2: ProcedureColumn) : ProcedureColumn = {
    Skip = c2.Skip |> Option.orElse c1.Skip
  }


module ProcedureRule =


  let fromDto fullYamlPath (dto: ProcedureRuleDto) : ProcedureRule = {
    IncludeOrFor =
      match dto.``include``, dto.``for`` with
      | None, None -> failwithYamlError fullYamlPath 0 0 "Either 'include' or 'for' is required in a procedure rule"
      | Some inc, None -> Include inc
      | None, Some f -> For f
      | Some _, Some _ -> failwithYamlError fullYamlPath 0 0 "'include' and 'for' may not be combined in a procedure rule"
    Except = dto.except
    Result =
      dto.result
      |> Option.map (function
          | "auto" -> Auto
          | "anonymous" -> AnonymousRecord
          | "nominal" -> NominalRecord
          | name -> Custom name
      )
    VoptionIn = dto.voptionIn
    VoptionOut = dto.voptionOut
    RecordIfSingleCol = dto.recordIfSingleCol
    SkipParamDto = dto.skipParamDto
    UseReturnValue = dto.useReturnValue
    Parameters =
      dto.``params``
      |> Option.defaultValue Map.empty
      |> Map.toList
      |> List.map (fun (k, v) -> if k = "" then None, v else Some k, v)
      |> Map.ofList
      |> Map.map (fun _ -> ProcedureParameter.fromDto)
    Columns =
      dto.columns
      |> Option.defaultValue Map.empty
      |> Map.toList
      |> List.map (fun (k, v) -> if k = "" then None, v else Some k, v)
      |> Map.ofList
      |> Map.map (fun _ -> ProcedureColumn.fromDto)
  }


  let defaultEffectiveRule : EffectiveProcedureRule = {
    Result = Auto
    VoptionIn = false
    VoptionOut = false
    RecordIfSingleCol = false
    SkipParamDto = false
    UseReturnValue = false
    ParametersFromAllRules = []
    ColumnsFromAllRules = []
  }


  let matches schemaName procName (rule: ProcedureRule) =
    let qualifiedName = $"%s{schemaName}.%s{procName}"
    let pattern =
      match rule.IncludeOrFor with
      | For x -> x
      | Include x -> x
    match rule.Except with
    | None -> Regex.IsMatch(qualifiedName, pattern)
    | Some exPattern -> Regex.IsMatch(qualifiedName, pattern) && not <| Regex.IsMatch(qualifiedName, exPattern)


  let merge (eff: EffectiveProcedureRule) (rule: ProcedureRule) : EffectiveProcedureRule =
    {
      Result = rule.Result |> Option.defaultValue eff.Result
      VoptionIn = rule.VoptionIn |> Option.defaultValue eff.VoptionIn
      VoptionOut = rule.VoptionOut |> Option.defaultValue eff.VoptionOut
      RecordIfSingleCol = rule.RecordIfSingleCol |> Option.defaultValue eff.RecordIfSingleCol
      SkipParamDto = rule.SkipParamDto |> Option.defaultValue eff.SkipParamDto
      UseReturnValue = rule.UseReturnValue |> Option.defaultValue eff.UseReturnValue
      ParametersFromAllRules = eff.ParametersFromAllRules @ [rule.Parameters]
      ColumnsFromAllRules = eff.ColumnsFromAllRules @ [rule.Columns]
    }


module EffectiveProcedureRule =


  let getParam paramName (rule: EffectiveProcedureRule) =
    rule.ParametersFromAllRules
    |> List.collect (fun map ->
        [
          yield! map.TryFind None |> Option.toList
          yield! map.TryFind (Some paramName) |> Option.toList
        ]
    )
    |> List.fold ProcedureParameter.merge ProcedureParameter.empty


  let getColumn colName (rule: EffectiveProcedureRule) =
    rule.ColumnsFromAllRules
    |> List.collect (fun map ->
        [
          yield! map.TryFind None |> Option.toList
          yield! map.TryFind (Some colName) |> Option.toList
        ]
    )
    |> List.fold ProcedureColumn.merge ProcedureColumn.empty


  let allColumnNames (rule: EffectiveProcedureRule) =
    rule.ColumnsFromAllRules
    |> List.collect (Map.toList >> List.map fst >> List.choose id)
    |> set


module ProcedureOrScriptParameter =

  let empty = {
    DtoName = None
    BuildValue = None
  }


  let fromProcedureParameter (p: ProcedureParameter) : ProcedureOrScriptParameter = {
      DtoName = p.DtoName
      BuildValue = p.BuildValue
  }


  let fromScriptParameter (p: ScriptParameter) : ProcedureOrScriptParameter = {
      DtoName = p.DtoName
      BuildValue = p.BuildValue
  }


  let merge (p1: ProcedureOrScriptParameter) (p2: ProcedureOrScriptParameter) : ProcedureOrScriptParameter = {
      DtoName = p2.DtoName |> Option.orElse p1.DtoName
      BuildValue = p2.BuildValue |> Option.orElse p1.DtoName
  }


module ProcedureOrScriptColumn =


  let fromProcedureColumn (p: ProcedureColumn) : ProcedureOrScriptColumn = {
      Skip = p.Skip
  }


  let fromScriptColumn (p: ScriptColumn) : ProcedureOrScriptColumn = {
      Skip = p.Skip
  }


module EffectiveProcedureOrScriptRule =


  let fromEffectiveProcedureRule (rule: EffectiveProcedureRule) : EffectiveProcedureOrScriptRule = {
      Result = rule.Result
      VoptionIn = rule.VoptionIn
      VoptionOut = rule.VoptionOut
      RecordIfSingleCol = rule.RecordIfSingleCol
      SkipParamDto = rule.SkipParamDto
      UseReturnValue = rule.UseReturnValue
      ParametersFromAllRules = rule.ParametersFromAllRules |> List.map (Map.map (fun _ -> ProcedureOrScriptParameter.fromProcedureParameter))
      ColumnsFromAllRules = rule.ColumnsFromAllRules |> List.map (Map.map (fun _ -> ProcedureOrScriptColumn.fromProcedureColumn))
  }


  let fromEffectiveScriptRule (rule: EffectiveScriptRule) : EffectiveProcedureOrScriptRule = {
      Result = rule.Result
      VoptionIn = rule.VoptionIn
      VoptionOut = rule.VoptionOut
      RecordIfSingleCol = rule.RecordIfSingleCol
      SkipParamDto = rule.SkipParamDto
      UseReturnValue = false
      ParametersFromAllRules = rule.ParametersFromAllRules |> List.map (Map.map (fun _ -> ProcedureOrScriptParameter.fromScriptParameter))
      ColumnsFromAllRules = rule.ColumnsFromAllRules |> List.map (Map.map (fun _ -> ProcedureOrScriptColumn.fromScriptColumn))
  }


  let getParam paramName (rule: EffectiveProcedureOrScriptRule) =
    rule.ParametersFromAllRules
    |> List.collect (fun map ->
        [
          yield! map.TryFind None |> Option.toList
          yield! map.TryFind (Some paramName) |> Option.toList
        ]
    )
    |> List.fold ProcedureOrScriptParameter.merge ProcedureOrScriptParameter.empty


module ScriptParameter =

  let empty = {
    Nullable = None
    Type = None
    DtoName = None
    BuildValue = None
  }


  let fromDto (dto: ScriptParameterDto) : ScriptParameter = {
    Nullable = dto.nullable
    Type = dto.``type``
    DtoName = dto.dtoName
    BuildValue = dto.buildValue
  }


  let merge (p1: ScriptParameter) (p2: ScriptParameter) = {
    Nullable = p2.Nullable |> Option.orElse p1.Nullable
    Type = p2.Type |> Option.orElse p1.Type
    DtoName = p2.DtoName |> Option.orElse p1.DtoName
    BuildValue = p2.BuildValue |> Option.orElse p1.BuildValue
  }


module ScriptTempTableRule =


  let fromDto basePath (dto: ScriptTempTableRuleDto) =
    {
      Definition =
        if dto.definition.EndsWith ".sql" && File.Exists(Path.Combine(basePath, dto.definition)) then
          File.ReadAllText(Path.Combine(basePath, dto.definition))
        else
          dto.definition
    }


module ScriptColumn =


  let empty : ScriptColumn =
    {
      Skip = None
    }


  let fromDto (dto: ScriptColumnDto) : ScriptColumn =
    {
      Skip = dto.skip
    }


  let merge (c1: ScriptColumn) (c2: ScriptColumn) : ScriptColumn = {
    Skip = c2.Skip |> Option.orElse c1.Skip
  }


module ScriptRule =


  let fromDto (basePath: string) fullYamlPath (dto: ScriptRuleDto) : ScriptRule =
    let exceptMatches =
      match dto.except with
      | Some pattern ->
          let matches = Glob.Files(basePath, pattern) |> set
          if matches.IsEmpty then
            logYamlWarning fullYamlPath 0 0 $"The 'except' glob pattern '{pattern}' does not match any files"
          matches
      | None -> Set.empty
    {
      IncludeOrFor =
        match dto.``include``, dto.``for`` with
        | None, None -> failwithYamlError fullYamlPath 0 0 "Either 'include' or 'for' is required in a script rule"
        | Some inc, None -> Include inc
        | None, Some f -> For f
        | Some _, Some _ -> failwithYamlError fullYamlPath 0 0 "'include' and 'for' may not be combined in a script rule"
      Except = dto.except
      IncludeMatches =
        match dto.``include`` with
        | None -> Set.empty
        | Some pattern ->
            let includeMatches = Glob.Files(basePath, pattern) |> set

            if includeMatches.IsEmpty then
              logYamlWarning fullYamlPath 0 0 $"The 'include' glob pattern '{pattern}' does not match any files"

            let finalMatches = includeMatches - exceptMatches

            if not includeMatches.IsEmpty && finalMatches.IsEmpty then
              logYamlWarning fullYamlPath 0 0 $"The 'include' glob pattern '{pattern}' does not match any files after subtracting the corresponding 'except' pattern '{dto.except.Value}'"

            finalMatches
      ForMatches =
        match dto.``for`` with
        | None -> Set.empty
        | Some pattern ->
            let forMatches = Glob.Files(basePath, pattern) |> set

            if forMatches.IsEmpty then
              logYamlWarning fullYamlPath 0 0 $"The 'for' glob pattern '{pattern}' does not match any files"

            let finalMatches = forMatches - exceptMatches

            if not forMatches.IsEmpty && finalMatches.IsEmpty then
              logYamlWarning fullYamlPath 0 0 $"The 'for' glob pattern '{pattern}' does not match any files after subtracting the corresponding 'except' pattern '{dto.except.Value}'"

            finalMatches
      Result =
        dto.result
        |> Option.map (function
            | "auto" -> Auto
            | "anonymous" -> AnonymousRecord
            | "nominal" -> NominalRecord
            | name -> Custom name
        )
      VoptionIn = dto.voptionIn
      VoptionOut = dto.voptionOut
      RecordIfSingleCol = dto.recordIfSingleCol
      SkipParamDto = dto.skipParamDto
      Parameters =
        dto.``params``
        |> Option.defaultValue Map.empty
        |> Map.toList
        |> List.map (fun (k, v) -> if k = "" then None, v else Some k, v)
        |> Map.ofList
        |> Map.map (fun _ -> ScriptParameter.fromDto)
      TempTables = dto.tempTables |> Option.map (List.map (ScriptTempTableRule.fromDto basePath))
      Columns =
        dto.columns
        |> Option.defaultValue Map.empty
        |> Map.toList
        |> List.map (fun (k, v) -> if k = "" then None, v else Some k, v)
        |> Map.ofList
        |> Map.map (fun _ -> ScriptColumn.fromDto)
    }


  let defaultEffectiveRule : EffectiveScriptRule = {
    Result = Auto
    VoptionIn = false
    VoptionOut = false
    RecordIfSingleCol = false
    SkipParamDto = false
    ParametersFromAllRules = []
    TempTables = []
    ColumnsFromAllRules = []
  }


  let matches (globMatchOutput: string) (rule: ScriptRule) =
    rule.IncludeMatches.Contains globMatchOutput
    || rule.ForMatches.Contains globMatchOutput


  let merge (eff: EffectiveScriptRule) (rule: ScriptRule) : EffectiveScriptRule =
    {
      Result = rule.Result |> Option.defaultValue eff.Result
      VoptionIn = rule.VoptionIn |> Option.defaultValue eff.VoptionIn
      VoptionOut = rule.VoptionOut |> Option.defaultValue eff.VoptionOut
      RecordIfSingleCol = rule.RecordIfSingleCol |> Option.defaultValue eff.RecordIfSingleCol
      SkipParamDto = rule.SkipParamDto |> Option.defaultValue eff.SkipParamDto
      ParametersFromAllRules = eff.ParametersFromAllRules @ [rule.Parameters]
      TempTables = rule.TempTables |> Option.defaultValue eff.TempTables
      ColumnsFromAllRules = eff.ColumnsFromAllRules @ [rule.Columns]
    }


module EffectiveScriptRule =


  let getParam paramName (rule: EffectiveScriptRule) =
    rule.ParametersFromAllRules
    |> List.collect (fun map ->
        [
          yield! map.TryFind None |> Option.toList
          yield! map.TryFind (Some paramName) |> Option.toList
        ]
    )
    |> List.fold ScriptParameter.merge ScriptParameter.empty


  let getColumn colName (rule: EffectiveScriptRule) =
    rule.ColumnsFromAllRules
    |> List.collect (fun map ->
        [
          yield! map.TryFind None |> Option.toList
          yield! map.TryFind (Some colName) |> Option.toList
        ]
    )
    |> List.fold ScriptColumn.merge ScriptColumn.empty


  let allParamNames (rule: EffectiveScriptRule) =
    rule.ParametersFromAllRules
    |> List.collect (Map.toList >> List.map fst >> List.choose id)
    |> set


  let allParams rule =
    allParamNames rule
    |> Set.map (fun name -> name, getParam name rule)
    |> Map.ofSeq


  let allColumnNames (rule: EffectiveScriptRule) =
    rule.ColumnsFromAllRules
    |> List.collect (Map.toList >> List.map fst >> List.choose id)
    |> set




module RuleSet =


  let fromDto projectDir resolveVariable fullYamlPath (dto: RuleSetDto) =
    let scriptBasePath =
      dto.scriptBasePath
      |> Option.map (fun scriptBase -> Path.Combine(projectDir, scriptBase))
      |> Option.defaultValue projectDir
      |> Path.GetFullPath
    {
      ConnectionString =
        lazy
          dto.connectionString
          |> Option.defaultWith (fun () -> failwithYamlError fullYamlPath 0 0 "All array items in the 'rulesets' section must have a 'connectionString' property")
          |> resolveVariable
      Filename = dto.filename |> Option.defaultValue "DbGen.fs"
      NamespaceOrModuleDeclaration = dto.namespaceOrModuleDeclaration |> Option.defaultValue "module DbGen"
      ScriptBasePath = scriptBasePath
      Prelude = dto.prelude |> Option.map String.getDeindentedLines
      TableDtos =
        dto.tableDtos
        |> Option.defaultValue []
        |> List.map (TableDtoRule.fromDto fullYamlPath)
      TableTypes =
        dto.tableTypes
        |> Option.defaultValue []
        |> List.map (TableTypeRule.fromDto fullYamlPath)
      Procedures =
        dto.procedures
        |> Option.defaultValue []
        |> List.map (ProcedureRule.fromDto fullYamlPath)
      Scripts =
        dto.scripts
        |> Option.defaultValue []
        |> List.map (ScriptRule.fromDto scriptBasePath fullYamlPath)
    }


  let shouldIncludeProcedure schemaName procedureName (cfg: RuleSet) =
    let qualifiedName = $"{schemaName}.{procedureName}"
    cfg.Procedures
    |> List.exists (fun rule ->
        match rule.IncludeOrFor, rule.Except with
        | For _, _ -> false
        | Include pattern, None -> Regex.IsMatch(qualifiedName, pattern)
        | Include incPattern, Some exPattern ->
            Regex.IsMatch(qualifiedName, incPattern) && not <| Regex.IsMatch(qualifiedName, exPattern)
    )


  let shouldIncludeTableDto schemaName tableName (cfg: RuleSet) =
    let qualifiedName = $"{schemaName}.{tableName}"
    cfg.TableDtos
    |> List.exists (fun rule ->
        match rule.IncludeOrFor, rule.Except with
        | For _, _ -> false
        | Include pattern, None -> Regex.IsMatch(qualifiedName, pattern)
        | Include incPattern, Some exPattern ->
            Regex.IsMatch(qualifiedName, incPattern) && not <| Regex.IsMatch(qualifiedName, exPattern)
    )


  let getEffectiveProcedureRuleFor schemaName procName (cfg: RuleSet) =
    cfg.Procedures
    |> List.filter (ProcedureRule.matches schemaName procName)
    |> List.fold ProcedureRule.merge ProcedureRule.defaultEffectiveRule


  let getEffectiveScriptRuleFor globMatchOutput (cfg: RuleSet) =
    cfg.Scripts
    |> List.filter (ScriptRule.matches globMatchOutput)
    |> List.fold ScriptRule.merge ScriptRule.defaultEffectiveRule


  let getEffectiveTableDtoRuleFor schemaName tableName (cfg: RuleSet) =
    cfg.TableDtos
    |> List.filter (TableDtoRule.matches schemaName tableName)
    |> List.fold TableDtoRule.merge TableDtoRule.defaultEffectiveRule


  let getEffectiveTableTypeRuleFor schemaName typeName (cfg: RuleSet) =
    cfg.TableTypes
    |> List.filter (TableTypeRule.matches schemaName typeName)
    |> List.fold TableTypeRule.merge TableTypeRule.defaultEffectiveRule



module FacilConfig =

  open Microsoft.Extensions.Configuration
  open Legivel.Serialization


  let getRuleSets projectDir fullYamlPath =

    let yaml = File.ReadAllText fullYamlPath

    let facilConfig =
      match DeserializeWithOptions<FacilConfigDto> [MappingMode MapYaml.AndRequireFullProjection] yaml with
      | [] -> failwithError "Unexpected empty YAML deserialization result"
      | [Success res] ->
          res.Warn
          |> List.iter (fun x ->
              logYamlWarning fullYamlPath x.Location.Line x.Location.Column x.Message
          )
          res.Data
      | results ->
          results
          |> List.choose (function Error err -> Some err | _ -> None)
          |> List.collect (fun ei ->
              [
                yield! ei.Error |> List.map (fun x -> x, logYamlError)
                yield! ei.Warn |> List.map (fun x -> x, logYamlWarning)
              ]
          )
          |> List.iter (fun (x, log) ->
              log fullYamlPath x.Location.Line x.Location.Column x.Message
          )
          failwithError "YAML deserialization failed"

    let configBuilder = ConfigurationBuilder()

    facilConfig.configs
    |> Option.iter (List.iter (fun (cfg: ConfigSourceDto) ->
        match cfg.appSettings, cfg.userSecrets, cfg.envVars with
        | Some path, None, None -> configBuilder.AddJsonFile (Path.Combine(projectDir, path)) |> ignore
        | None, Some secretsId, None -> configBuilder.AddUserSecrets secretsId |> ignore
        | None, None, Some (null | "") -> configBuilder.AddEnvironmentVariables() |> ignore
        | None, None, Some prefix -> configBuilder.AddEnvironmentVariables(prefix) |> ignore
        | _ -> failwithYamlError fullYamlPath 0 0 "Each array item in the 'configs' section of the YAML may only contain one property (either 'appSettings', 'userSecrets', or 'envVars')"
    ))

    let config = configBuilder.Build()

    let resolveVariable (str: string) =
      let m = Regex.Match(str, "\$\((.+)\)")
      if not m.Success then str
      else
        match facilConfig.configs with
        | None | Some [] -> failwithYamlError fullYamlPath 0 0 $"Cannot use variable {str} since no configuration sources has been specified"
        | _ -> ()
        let varName = m.Groups.[1].Value
        match config.GetValue varName with
        | null -> failwithYamlError fullYamlPath 0 0 $"The variable {str} could not be found in the specified configuration sources"
        | str -> str

    match facilConfig.rulesets with
    | None -> failwithYamlError fullYamlPath 0 0 "The 'rulesets' section is required"
    | Some [] -> failwithYamlError fullYamlPath 0 0 "At least one item must be specified in the 'rulesets' section"
    | Some rulesets -> rulesets |> List.map (RuleSet.fromDto projectDir resolveVariable fullYamlPath)
