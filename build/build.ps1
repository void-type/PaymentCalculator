[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $SkipFormat,
  [switch] $SkipOutdated,
  [switch] $SkipTest,
  [switch] $SkipTestReport,
  [switch] $SkipPublish
)

Push-Location $PSScriptRoot

# Clean the artifacts folders
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

# Restore local dotnet tools
Push-Location -Path "../"
dotnet tool restore
Pop-Location

. ./util.ps1

# Build solution
Push-Location -Path "../"

if (-not $SkipFormat) {
  dotnet format --check
  Stop-OnError
}

dotnet restore

if (-not $SkipOutdated) {
  dotnet outdated
}

dotnet build --configuration "$Configuration" --no-restore

Stop-OnError
Pop-Location

if (-not $SkipTest) {
  # Run tests, gather coverage
  Push-Location -Path "$testProjectFolder"

  dotnet test `
    --configuration "$Configuration" `
    --no-build `
    --results-directory '../../testResults' `
    --logger 'trx' `
    --collect:"XPlat Code Coverage"

  Stop-OnError

  if (-not $SkipTestReport) {
    # Generate code coverage report
    dotnet reportgenerator `
      "-reports:../../testResults/*/coverage.cobertura.xml" `
      "-targetdir:../../coverage" `
      "-reporttypes:HtmlInline_AzurePipelines"

    Stop-OnError
  }

  Pop-Location
}

if (-not $SkipPublish) {
  # Package build
  Push-Location -Path "$blazorWasmProjectFolder"

  dotnet publish --configuration "$Configuration" --output "../../artifacts/blazorWasm"

  Stop-OnError
  Pop-Location

  Push-Location -Path "$wpfProjectFolder"

  dotnet publish --configuration "$Configuration" --output "../../artifacts/portable" `
    --runtime "win-x64" `
    --self-contained true `
    /p:PublishSingleFile=true `
    /p:PublishReadyToRun=true `
    /p:IncludeNativeLibrariesForSelfExtract=true
  # TODO: https://github.com/dotnet/sdk/issues/14261
  # /p:PublishTrimmed=true

  dotnet publish --configuration "$Configuration" --output "../../artifacts/framework" `
    --runtime "win-x64" `
    /p:PublishSingleFile=true
  # TODO: https://github.com/dotnet/sdk/issues/14261
  # /p:PublishTrimmed=true

  Stop-OnError
  Pop-Location
}

Pop-Location


Write-Host "`nBuilt $projectName $projectVersion`n"
