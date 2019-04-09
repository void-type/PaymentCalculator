[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $SkipTestReport
)

./build.ps1 -Configuration $Configuration -SkipClient -SkipTestReport:$SkipTestReport -SkipPublish
