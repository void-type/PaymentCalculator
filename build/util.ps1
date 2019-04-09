function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Exit $LASTEXITCODE
  }
}

$projectName = "PaymentCalculator"

$testProjectFolder = "../tests/$projectName.Test"
$wpfProjectFolder = "../src/$projectName.Wpf"
