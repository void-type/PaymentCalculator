function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Exit $LASTEXITCODE
  }
}

$shortAppName = "PaymentCalculator"
$projectName = "$($shortAppName)"

$testProjectFolder = "../tests/$projectName.Test"
$wpfProjectFolder = "../src/$projectName.Wpf"
