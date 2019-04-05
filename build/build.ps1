[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $SkipTest,
  [switch] $SkipPack
)

. ./util.ps1

# Clean the artifacts folder
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue

# Clean coverage folder
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue

# Clean testResults folder
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

# Build solution
Push-Location -Path "../"
dotnet build --configuration "$Configuration"
Stop-OnError
Pop-Location

if (-not $SkipTest) {
  # Run tests, gather coverage
  Push-Location -Path "../tests/PaymentCalculator.Test"

  dotnet test `
    --configuration "$Configuration" `
    --no-build `
    --logger 'trx' `
    --results-directory '../../testResults' `
    /p:Exclude='[xunit.runner.*]*' `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput="../../coverage/coverage.cobertura.xml"

  Stop-OnError
  Pop-Location

  # TODO: Not supported on .Net Core 3.0 yet
  # Generate code coverage report
  # Push-Location -Path "../coverage"
  # reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:." "-reporttypes:HtmlInline_AzurePipelines"
  # Stop-OnError
  # Pop-Location
}

if (-not $SkipPublish) {
  # Create Publish folder
  Get-ChildItem -Path "../src" |
    Where-Object { (Test-Path -Path "$($_.FullName)/*.csproj") -eq $true } |
    Select-Object -ExpandProperty Name |
    ForEach-Object {
    Push-Location -Path "../src/$_"
    InheritDoc --base "./bin/$Configuration/" --overwrite
    Stop-OnError
    dotnet publish --configuration "$Configuration" --no-build --output "../../artifacts/pre-release" /p:PublicRelease=false
    dotnet publish --configuration "$Configuration" --no-build --output "../../artifacts"
    Stop-OnError
    Pop-Location
  }
}
