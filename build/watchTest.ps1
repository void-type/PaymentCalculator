Push-Location -Path "$PSScriptRoot/../"
. ./build/util.ps1

try {
  dotnet watch test --project "$testProjectFolder" --configuration 'Debug'

} finally {
  Pop-Location
}
