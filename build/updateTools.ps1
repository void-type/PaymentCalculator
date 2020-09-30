# TODO: Use this script until dotnet tool update supports --all command
# https://github.com/dotnet/sdk/issues/10130

Push-Location $PSScriptRoot

$json = Get-Content ../.config/dotnet-tools.json |
  ConvertFrom-Json

$json.tools.psobject.Properties.Name |
  ForEach-Object { dotnet tool update $_ --framework netcoreapp3.1 }

  Pop-Location
