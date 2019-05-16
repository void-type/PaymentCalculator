[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $SkipTest,
  [switch] $SkipTestReport,
  [switch] $SkipPublish
)

. ./util.ps1

# Clean the artifacts folders
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

# Build solution
Push-Location -Path "../"
dotnet format --check
Stop-OnError
dotnet-outdated
dotnet build --configuration "$Configuration"
Stop-OnError
Pop-Location

if (-not $SkipTest) {
  # Run tests, gather coverage
  Push-Location -Path "$testProjectFolder"

  dotnet test `
    --configuration "$Configuration" `
    --no-build `
    --logger 'trx' `
    --results-directory '../../testResults' `
    /p:Exclude="[xunit.*]*%2c[$projectName.Test]*" `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput="../../coverage/coverage.cobertura.xml"

  Stop-OnError
  Pop-Location

  if (-not $SkipTestReport) {
    # Generate code coverage report
    Push-Location -Path "../coverage"
    reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:." "-reporttypes:HtmlInline_AzurePipelines"
    Stop-OnError
    Pop-Location
  }
}

if (-not $SkipPublish) {
  # Package build
  Push-Location -Path "$wpfProjectFolder"
  dotnet publish --configuration "$Configuration" --no-build --output "../../artifacts"
  Stop-OnError
  Pop-Location
}
