[CmdletBinding(DefaultParameterSetName = 'Check')]
param(
    [Parameter(Mandatory, ParameterSetName = 'Apply')][switch] $Apply,
    [Parameter(Mandatory, ParameterSetName = 'Check')][switch] $Check,
    [Parameter(Mandatory)][string] $ProjectDirectory,
    [string] $RepoRoot = (Get-Location).Path
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

try {
    if ([IO.Path]::IsPathRooted($ProjectDirectory) -or
        $ProjectDirectory -match '(^|[\\/])\.\.([\\/]|$)' -or
        $ProjectDirectory -notmatch '^[A-Za-z0-9._/-]+$') {
        throw "Unsafe project directory: $ProjectDirectory"
    }

    $skillRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
    $repoRootPath = [IO.Path]::GetFullPath($RepoRoot)
    $templateRoot = Join-Path $skillRoot 'assets/repository'
    $files = @(
        @{
            Source = '.gitignore.template'
            Destination = '.gitignore'
            Replacements = @{ '@@PROJECT_DIRECTORY@@' = $ProjectDirectory }
        },
        @{
            Source = '.markdownlint-cli2.yaml'
            Destination = '.markdownlint-cli2.yaml'
            Replacements = @{}
        }
    )

    foreach ($entry in $files) {
        $expected = [IO.File]::ReadAllText((Join-Path $templateRoot $entry.Source))
        foreach ($replacement in $entry.Replacements.GetEnumerator()) {
            $expected = $expected.Replace($replacement.Key, $replacement.Value)
        }
        if ($expected -match '@@[A-Z0-9_]+@@') {
            throw "Unresolved repository template token in $($entry.Source)."
        }
        $expected = $expected.Replace("`r`n", "`n").Replace("`r", "`n")
        $destination = Join-Path $repoRootPath $entry.Destination

        if ($Apply) {
            [IO.File]::WriteAllText($destination, $expected, [Text.UTF8Encoding]::new($false))
            Write-Output "rendered $($entry.Destination)"
        }
        else {
            $actual = if (Test-Path -LiteralPath $destination -PathType Leaf) {
                [IO.File]::ReadAllText($destination).Replace("`r`n", "`n").Replace("`r", "`n")
            }
            else {
                $null
            }
            if ($null -eq $actual -or $actual -cne $expected) {
                [Console]::Error.WriteLine("drifted $($entry.Destination)")
                exit 1
            }
            Write-Output "matched $($entry.Destination)"
        }
    }
}
catch {
    [Console]::Error.WriteLine("Repository render failed: $($_.Exception.Message)")
    exit 2
}
