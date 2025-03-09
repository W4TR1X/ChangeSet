# Call Sample
# sed -i 's/\r//g' ./run-action.ps1 && pwsh -File ./run-action.ps1 -config "./changeset.config.json" -pairs "-version=1010 -buildNumber=1235ee"

param (
	[string]$path,
	[string]$config,
	[string]$pairs
)

if ($path -eq "") {
	Write-Output "No path provided, defaulting to $PSScriptRoot"
	$path = $PSScriptRoot
}

if ($config -eq "") {
	Write-Output "No config provided, defaulting to ./changeset.config.json"
	$config = "./changeset.config.json"
}

if ($config.startsWith("./")) {
	$config = Join-Path $PSScriptRoot $config.Substring(2)
	Write-Output "Normalize config path $config"
}

if ($IsWindows -eq $True) {
	Write-Output "Running on Windows"
	$filePath = Join-Path $PSScriptRoot '/dist/win-x64/ChangeSet.exe'
}

if ($IsLinux -eq $True) {
	Write-Output "Running on Linux"
	$filePath = Join-Path $PSScriptRoot '/dist/linux-x64/ChangeSet'	
	chmod +x $filePath
}

if ($IsMacOS -eq $True) {
	Write-Output "Running on MacOS"
	$filePath = Join-Path $PSScriptRoot '/dist/osx-x64/ChangeSet'
	chmod +x $filePath
}

if ($filePath -eq $null) {
	Write-Output "Unsupported OS"
	exit 1
}

$arguments = "-path='$path'", "-config='$config'", $pairs
$command = "$filePath $arguments"
Write-Output $command

Invoke-Expression $command

if ($LASTEXITCODE -ne 0) {
	exit $LASTEXITCODE
}