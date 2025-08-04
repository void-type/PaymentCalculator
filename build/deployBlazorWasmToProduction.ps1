# Run this script as a server administrator from the scripts directory
[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = 'High')]
param()

function Stop-OnError([string]$errorMessage) {
  if ($LASTEXITCODE -eq 0) {
    return
  }

  if (-not [string]::IsNullOrWhiteSpace($errorMessage)) {
    Write-Error $errorMessage
  }

  exit $LASTEXITCODE
}

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  if (-not (Test-Path -Path $blazorWasmReleaseFolder)) {
    throw 'No artifacts to deploy. Run build.ps1 before deploying.'
  }

  if (-not (Test-Path -Path $blazorWasmDirectoryProduction)) {
    throw "No production directory found at $blazorWasmDirectoryProduction"
  }

  ROBOCOPY "$blazorWasmReleaseFolder/_framework" "$blazorWasmDirectoryProduction/_framework" /MIR /XF
  ROBOCOPY "$blazorWasmReleaseFolder/css" "$blazorWasmDirectoryProduction/css" /MIR /XF

} finally {
  Set-Location $originalLocation
}
