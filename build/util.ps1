function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Exit $LASTEXITCODE
  }
}

$projectName = "PaymentCalculator"
$projectVersion = (dotnet nbgv get-version -f json | ConvertFrom-Json).NuGetPackageVersion

$testProjectFolder = "../tests/$projectName.Test"
$wpfProjectFolder = "../src/$projectName.Wpf"
$blazorWasmProjectFolder = "../src/$projectName.BlazorWasm"
