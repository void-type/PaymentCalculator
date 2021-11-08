[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $SkipFormat,
  [switch] $SkipOutdated,
  [switch] $SkipTest,
  [switch] $SkipTestReport,
  [switch] $SkipPublish
)

Push-Location -Path "$PSScriptRoot/../"
. ./build/util.ps1

try {
  # Clean the artifacts folder
  Remove-Item -Path "./artifacts" -Recurse -ErrorAction SilentlyContinue

  # Restore local dotnet tools
  dotnet tool restore

  # Build solution
  if (-not $SkipFormat) {
    # TODO: temporarily disabled due to bug: https://github.com/dotnet/format/issues/1337
    dotnet format --verify-no-changes --exclude './src/PaymentCalculator.Wpf/'
    if ($LASTEXITCODE -ne 0) {
      Write-Error 'Please run formatter: dotnet format.'
    }
    Stop-OnError
  }

  dotnet restore

  if (-not $SkipOutdated) {
    dotnet outdated
  }

  dotnet build --configuration "$Configuration" --no-restore
  Stop-OnError

  if (-not $SkipTest) {
    # Run tests, gather coverage
    dotnet test "$testProjectFolder" `
      --configuration "$Configuration" `
      --no-build `
      --results-directory './artifacts/testResults' `
      --logger 'trx' `
      --collect:'XPlat Code Coverage'

    Stop-OnError

    if (-not $SkipTestReport) {
      # Generate code coverage report
      dotnet reportgenerator `
        '-reports:./artifacts/testResults/*/coverage.cobertura.xml' `
        '-targetdir:./artifacts/testCoverage' `
        '-reporttypes:HtmlInline_AzurePipelines'

      Stop-OnError
    }
  }

  if (-not $SkipPublish) {
    # Package build

    dotnet publish "$blazorWasmProjectFolder" --configuration "$Configuration" --output "./artifacts/dist/release/blazorWasm"
    Stop-OnError

    dotnet publish "$wpfProjectFolder" --configuration "$Configuration" --output "./artifacts/dist/release/portable" `
      --runtime "win-x64" `
      --self-contained true `
      /p:PublishSingleFile=true `
      /p:PublishReadyToRun=true `
      /p:IncludeNativeLibrariesForSelfExtract=true
    # TODO: https://github.com/dotnet/sdk/issues/14261
    # /p:PublishTrimmed=true

    dotnet publish "$wpfProjectFolder" --configuration "$Configuration" --output "./artifacts/dist/release/framework" `
      --runtime "win-x64" `
      /p:PublishSingleFile=true
    # TODO: https://github.com/dotnet/sdk/issues/14261
    # /p:PublishTrimmed=true

    Stop-OnError
    Pop-Location
  }

  $projectVersion = (dotnet nbgv get-version -f json | ConvertFrom-Json).NuGetPackageVersion
  Write-Output "`nBuilt $projectName $projectVersion`n"

} finally {
  Pop-Location
}
