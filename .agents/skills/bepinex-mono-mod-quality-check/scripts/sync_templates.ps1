[CmdletBinding(DefaultParameterSetName = 'Check')]
param(
    [Parameter(Mandatory, ParameterSetName = 'Apply')]
    [switch] $Apply,

    [Parameter(Mandatory, ParameterSetName = 'Check')]
    [switch] $Check,

    [Parameter()]
    [string] $RepoRoot = (Get-Location).Path,

    [Parameter()]
    [string[]] $Template
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

function ConvertFrom-SafeRelativePath {
    param(
        [Parameter(Mandatory)]
        [string] $Value,

        [Parameter(Mandatory)]
        [string] $Field
    )

    if ([string]::IsNullOrWhiteSpace($Value) -or [IO.Path]::IsPathRooted($Value)) {
        throw "Unsafe ${Field}: '$Value'"
    }

    $parts = @($Value -split '[/\\]')
    if ($parts.Count -eq 0 -or $parts -contains '..' -or $parts -contains '') {
        throw "Unsafe ${Field}: '$Value'"
    }
    $relativePath = $parts[0]
    foreach ($part in $parts | Select-Object -Skip 1) {
        $relativePath = Join-Path $relativePath $part
    }
    return $relativePath
}

function Test-FileContentEqual {
    param(
        [Parameter(Mandatory)]
        [string] $Left,

        [Parameter(Mandatory)]
        [string] $Right,

        [Parameter(Mandatory)]
        [ValidateSet('bytes', 'text-normalized-newlines')]
        [string] $Comparison
    )

    if ($Comparison -eq 'text-normalized-newlines') {
        $leftText = [IO.File]::ReadAllText($Left).Replace("`r`n", "`n").Replace("`r", "`n")
        $rightText = [IO.File]::ReadAllText($Right).Replace("`r`n", "`n").Replace("`r", "`n")
        return $leftText -ceq $rightText
    }

    $leftBytes = [IO.File]::ReadAllBytes($Left)
    $rightBytes = [IO.File]::ReadAllBytes($Right)
    if ($leftBytes.Length -ne $rightBytes.Length) {
        return $false
    }
    for ($index = 0; $index -lt $leftBytes.Length; $index++) {
        if ($leftBytes[$index] -ne $rightBytes[$index]) {
            return $false
        }
    }
    return $true
}

try {
    $skillRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
    $assetsRoot = [IO.Path]::GetFullPath((Join-Path $skillRoot 'assets'))
    $manifestPath = Join-Path $assetsRoot 'template-map.json'
    $manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
    if ($manifest.schema_version -ne 1) {
        throw 'Unsupported template-map schema_version.'
    }

    $repoRootPath = [IO.Path]::GetFullPath($RepoRoot)
    if (-not (Test-Path -LiteralPath $repoRootPath -PathType Container)) {
        throw "Repository root does not exist: $repoRootPath"
    }

    $seenIds = [Collections.Generic.HashSet[string]]::new(
        [StringComparer]::Ordinal
    )
    $seenDestinations = [Collections.Generic.HashSet[string]]::new(
        [StringComparer]::Ordinal
    )
    $templates = @()
    foreach ($entry in @($manifest.templates)) {
        $templateId = [string] $entry.id
        $sourceRelative = ConvertFrom-SafeRelativePath `
            -Value ([string] $entry.source) -Field 'source'
        $destinationRelative = ConvertFrom-SafeRelativePath `
            -Value ([string] $entry.destination) -Field 'destination'
        $comparison = [string] $entry.comparison
        if ($comparison -notin @('bytes', 'text-normalized-newlines')) {
            throw "Unsupported comparison for ${templateId}: '$comparison'"
        }
        if (-not $seenIds.Add($templateId)) {
            throw "Duplicate template id: $templateId"
        }
        if (-not $seenDestinations.Add($destinationRelative)) {
            throw "Duplicate template destination: $destinationRelative"
        }

        $sourcePath = Join-Path $assetsRoot $sourceRelative
        if (-not (Test-Path -LiteralPath $sourcePath -PathType Leaf)) {
            throw "Template source does not exist: $sourcePath"
        }
        $templates += [pscustomobject]@{
            Id = $templateId
            Source = $sourcePath
            Destination = $destinationRelative
            Comparison = $comparison
        }
    }
    if ($templates.Count -eq 0) {
        throw 'template-map contains no templates.'
    }

    if ($Template) {
        $unknownIds = @($Template | Where-Object { -not $seenIds.Contains($_) })
        if ($unknownIds.Count -gt 0) {
            throw "Unknown template id(s): $($unknownIds -join ', ')"
        }
        $selectedIds = [Collections.Generic.HashSet[string]]::new(
            $Template,
            [StringComparer]::Ordinal
        )
        $templates = @($templates | Where-Object { $selectedIds.Contains($_.Id) })
    }

    $drifted = $false
    foreach ($entry in $templates) {
        $destinationPath = Join-Path $repoRootPath $entry.Destination
        $displayPath = $entry.Destination.Replace('\', '/')
        if ($Apply) {
            [IO.Directory]::CreateDirectory(
                [IO.Path]::GetDirectoryName($destinationPath)
            ) | Out-Null
            [IO.File]::Copy($entry.Source, $destinationPath, $true)
            Write-Output "applied $($entry.Id): $displayPath"
            continue
        }

        if (-not (Test-Path -LiteralPath $destinationPath -PathType Leaf)) {
            [Console]::Error.WriteLine("missing $($entry.Id): $displayPath")
            $drifted = $true
        }
        elseif (-not (Test-FileContentEqual `
            -Left $entry.Source `
            -Right $destinationPath `
            -Comparison $entry.Comparison
        )) {
            [Console]::Error.WriteLine("drifted $($entry.Id): $displayPath")
            $drifted = $true
        }
        else {
            Write-Output "matched $($entry.Id): $displayPath"
        }
    }

    if ($drifted) {
        exit 1
    }
}
catch {
    [Console]::Error.WriteLine(
        "Template sync failed: $($_.Exception.Message)"
    )
    exit 2
}
