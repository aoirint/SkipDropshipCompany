# APM installation templates

| Platform | `platform` in `apm-bootstrap.json` |
| --- | --- |
| macOS Apple Silicon | `macos-arm64` |
| macOS Intel | `macos-x86_64` |
| Linux ARM64 | `linux-arm64` |
| Linux x86_64 | `linux-x86_64` |
| Windows x86_64 | `windows-x86_64` |

Copy `version`, `archive`, `sha256`, and `extracted_directory` from the selected
manifest row. Build `url` from `download_url_template`.

## macOS

```sh
workdir="$(mktemp -d)"
curl --fail --location --proto '=https' --tlsv1.2 -o "$workdir/$archive" "$url"
actual="$(shasum -a 256 "$workdir/$archive" | awk '{print $1}')"
test "$actual" = "$sha256" || { rm -f "$workdir/$archive"; exit 1; }
test ! -e "$HOME/.local/lib/apm" || { echo 'Existing APM directory; stop.' >&2; exit 1; }
test ! -e "$HOME/.local/bin/apm" || { echo 'Existing APM command; stop.' >&2; exit 1; }
mkdir -p "$HOME/.local/lib/apm" "$HOME/.local/bin"
tar -xzf "$workdir/$archive" -C "$workdir"
cp -R "$workdir/$extracted_directory/." "$HOME/.local/lib/apm/"
ln -s "$HOME/.local/lib/apm/apm" "$HOME/.local/bin/apm"
"$HOME/.local/bin/apm" --version
```

## Linux

```sh
workdir="$(mktemp -d)"
curl --fail --location --proto '=https' --tlsv1.2 -o "$workdir/$archive" "$url"
printf '%s  %s\n' "$sha256" "$workdir/$archive" | sha256sum --check --strict
test ! -e "$HOME/.local/lib/apm" || { echo 'Existing APM directory; stop.' >&2; exit 1; }
test ! -e "$HOME/.local/bin/apm" || { echo 'Existing APM command; stop.' >&2; exit 1; }
mkdir -p "$HOME/.local/lib/apm" "$HOME/.local/bin"
tar -xzf "$workdir/$archive" -C "$workdir"
cp -R "$workdir/$extracted_directory/." "$HOME/.local/lib/apm/"
ln -s "$HOME/.local/lib/apm/apm" "$HOME/.local/bin/apm"
"$HOME/.local/bin/apm" --version
```

## Windows x86_64

```powershell
$workdir = Join-Path ([IO.Path]::GetTempPath()) "apm-$version"
$archivePath = Join-Path $workdir $archive
$installDir = Join-Path $env:LOCALAPPDATA 'Programs\apm'
if (Test-Path -LiteralPath $installDir) { throw 'Existing APM directory; stop.' }
New-Item -ItemType Directory -Force -Path $workdir, $installDir | Out-Null
Invoke-WebRequest -Uri $url -OutFile $archivePath
$actual = (Get-FileHash -LiteralPath $archivePath -Algorithm SHA256).Hash.ToLowerInvariant()
if ($actual -ne $sha256) { Remove-Item -LiteralPath $archivePath; throw 'APM checksum mismatch.' }
Expand-Archive -LiteralPath $archivePath -DestinationPath $workdir
Copy-Item -Path (Join-Path $workdir "$extracted_directory\*") -Destination $installDir -Recurse
$env:Path = "$installDir;$env:Path"
apm --version
```
