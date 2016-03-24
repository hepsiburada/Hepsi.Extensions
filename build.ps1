Param(
	[string]$buildNumber = "0",
	[string]$preRelease = $null
)

Import-Module .\packages\psake.4.4.2\tools\psake.psm1

if(Test-Path Env:\APPVEYOR_BUILD_NUMBER){
	$buildNumber = [int]$Env:APPVEYOR_BUILD_NUMBER
	Write-Host "Using APPVEYOR_BUILD_NUMBER"

	$task = "appVeyor"
}

"Build number $buildNumber"

Invoke-Psake .\default.ps1 $task -framework "4.0x86" -properties @{ buildNumber=$buildNumber; preRelease=$preRelease }

Remove-Module psake
