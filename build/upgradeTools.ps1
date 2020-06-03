# TODO: Use this script until dotnet tool update supports --all command
# https://github.com/dotnet/sdk/issues/10130

$json = Get-Content ../.config/dotnet-tools.json |
  ConvertFrom-Json

$json.tools.psobject.Properties.Name |
  ForEach-Object { dotnet tool update $_ }
