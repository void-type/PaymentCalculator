# TODO: Use this script until dotnet tool update supports --all command
# https://github.com/dotnet/sdk/issues/10130

$manifestPath = "$PSScriptRoot/../.config/dotnet-tools.json"

$json = Get-Content -Path $manifestPath |
  ConvertFrom-Json

$json.tools.PSObject.Properties.Name |
  ForEach-Object {
    dotnet tool update $_ --tool-manifest $manifestPath
  }
