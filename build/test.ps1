[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $SkipTestReport
)

./build.ps1 -Configuration $Configuration -SkipTestReport:$SkipTestReport -SkipPublish
