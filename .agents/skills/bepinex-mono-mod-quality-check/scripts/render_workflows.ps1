[CmdletBinding(DefaultParameterSetName = 'Check')]
param(
    [Parameter(Mandatory, ParameterSetName = 'Apply')][switch] $Apply,
    [Parameter(Mandatory, ParameterSetName = 'Check')][switch] $Check,
    [Parameter(Mandatory)][string] $VariablesFile,
    [string] $RepoRoot = (Get-Location).Path
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

function Get-RenderedTemplate([string] $TemplatePath, [psobject] $Variables) {
    $text = [IO.File]::ReadAllText($TemplatePath)
    $required = @(
        'project_directory',
        'project_file',
        'plugin_assembly',
        'thunderstore_namespace',
        'thunderstore_community',
        'thunderstore_categories'
    )
    foreach ($name in $required) {
        $value = $Variables.$name
        if ($null -eq $value) { throw "Missing workflow variable: $name" }
        if ($name -eq 'thunderstore_categories') {
            $values = @($value) | ForEach-Object { [string] $_ }
            if ($values.Count -eq 0 -or ($values | Where-Object { [string]::IsNullOrWhiteSpace($_) -or $_ -match "`r|`n" })) { throw "Unsafe workflow variable: $name" }
            $value = $values -join "`n            "
        }
        else {
            $value = [string] $value
            if ([string]::IsNullOrWhiteSpace($value) -or $value -match "`r|`n") { throw "Unsafe workflow variable: $name" }
        }
        if ($name -ne 'thunderstore_categories' -and [string]::IsNullOrWhiteSpace($value)) {
            throw "Unsafe workflow variable: $name"
        }
        $text = $text.Replace("@@$($name.ToUpperInvariant())@@", $value)
    }
    if ($text -match '@@[A-Z0-9_]+@@') { throw 'Unresolved workflow template token.' }
    return $text.Replace("`r`n", "`n").Replace("`r", "`n")
}

try {
    $skillRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
    $repoRootPath = [IO.Path]::GetFullPath($RepoRoot)
    $variables = Get-Content -LiteralPath $VariablesFile -Raw | ConvertFrom-Json
    $templates = @(
        @{ Source = 'build.yml.template'; Destination = '.github/workflows/build.yml' },
        @{ Source = 'lint.yml.template'; Destination = '.github/workflows/lint.yml' },
        @{ Source = '.gitignore.template'; Destination = '.gitignore' },
        @{ Source = '.markdownlint-cli2.yaml'; Destination = '.markdownlint-cli2.yaml' }
    )
    foreach ($entry in $templates) {
        $templateRoot = if ($entry.Source -in @('.gitignore.template', '.markdownlint-cli2.yaml')) { Join-Path $skillRoot 'assets/repository' } else { Join-Path $skillRoot 'assets/github/workflows' }
        $expected = Get-RenderedTemplate (Join-Path $templateRoot $entry.Source) $variables
        $destination = Join-Path $repoRootPath $entry.Destination
        if ($Apply) {
            [IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($destination)) | Out-Null
            [IO.File]::WriteAllText($destination, $expected, [Text.UTF8Encoding]::new($false))
            Write-Output "rendered $($entry.Destination)"
        }
        elseif (-not (Test-Path -LiteralPath $destination -PathType Leaf) -or ([IO.File]::ReadAllText($destination).Replace("`r`n", "`n").Replace("`r", "`n") -cne $expected)) {
            [Console]::Error.WriteLine("drifted $($entry.Destination)")
            exit 1
        }
        else { Write-Output "matched $($entry.Destination)" }
    }
}
catch { [Console]::Error.WriteLine("Workflow render failed: $($_.Exception.Message)"); exit 2 }
