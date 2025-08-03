$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  dotnet watch --project "$blazorWasmProjectFolder" --configuration 'Debug'

} finally {
  Set-Location $originalLocation
}
