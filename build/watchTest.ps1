. ./util.ps1

Push-Location -Path "$testProjectFolder"
dotnet watch test --configuration "Debug"
Pop-Location
