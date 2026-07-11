# Icon Authoring

`assets/icon.svg` is the editable source for the package icon.
`assets/icon.png` is the 256×256 Thunderstore asset generated from that SVG.
Do not edit the PNG by hand.

## SVG source

- Keep the SVG viewport and background at 256×256.
- Keep text centered unless the design intentionally requires another layout.
- Use the XCF-derived source font with the family and weight below:

  ```svg
  font-family="'源柔ゴシック', 'Gen Jyuu Gothic', sans-serif"
  font-weight="700"
  ```

  `Bold` is a font weight, not part of the browser-recognized family name.
  Using `Gen Jyuu Gothic Bold` as a family name falls back to another font and
  loses the rounded glyph shapes.
- Keep a generic fallback so the SVG remains readable when the source font is
  unavailable. Regenerate the committed PNG only on a machine where the source
  font is installed.

## Regenerating the PNG

Render at four times the target resolution, then resize with high-quality
bicubic interpolation. Rendering at 256×256 directly produces rougher text
edges.

The following PowerShell script uses Microsoft Edge and `System.Drawing`:

```powershell
$edge = "${env:ProgramFiles(x86)}\Microsoft\Edge\Application\msedge.exe"
$svg = (Resolve-Path "assets/icon.svg").Path -replace '\\', '/'
$png = Join-Path (Get-Location) "assets/icon.png"
$highResolutionPng = Join-Path $env:TEMP "package-icon-4x.png"
$profile = Join-Path $env:TEMP "package-icon-render-profile"

Remove-Item $highResolutionPng, $profile -Recurse -Force -ErrorAction SilentlyContinue
Start-Process -FilePath $edge -ArgumentList "--headless --disable-gpu --force-device-scale-factor=4 --user-data-dir=$profile --run-all-compositor-stages-before-draw --virtual-time-budget=1000 --screenshot=$highResolutionPng --window-size=256,256 file:///$svg" -Wait

Add-Type -AssemblyName System.Drawing
$source = [System.Drawing.Bitmap]::new($highResolutionPng)
$target = [System.Drawing.Bitmap]::new(256, 256, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$graphics = [System.Drawing.Graphics]::FromImage($target)
try {
    $graphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
    $graphics.DrawImage($source, [System.Drawing.Rectangle]::new(0, 0, 256, 256))
    $target.Save($png, [System.Drawing.Imaging.ImageFormat]::Png)
}
finally {
    $graphics.Dispose()
    $target.Dispose()
    $source.Dispose()
}
```

## Verification

1. Confirm `assets/icon.png` is 256×256.
2. Inspect the generated PNG at native size for correct font selection,
   centered layout, smooth edges, and unwanted color fringes.
3. Keep the SVG and generated PNG together in the same commit.
