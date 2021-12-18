$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/util.ps1

  dotnet watch test --project "$testProjectFolder" --configuration 'Debug'

} finally {
  Set-Location $originalLocation
}
