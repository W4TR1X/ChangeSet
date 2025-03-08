# Call Sample
# sed -i 's/\r//g' .\run-action.ps1 && pwsh -File .\run-action.ps1 -config "./changeset.config.json" -pairs "-version=1010 -buildNumber=1235ee"

param (
    [string]$path,
		[string]$config,
		[string]$pairs
)

if($path -eq "") {
		Write-Output "No path provided, defaulting to $PSScriptRoot"
		$path = $PSScriptRoot
}

if($config -eq "") {
		Write-Output "No config provided, defaulting to ./changeset.config.json"
		$config = "./changeset.config.json"
}elseif($config.startsWith("./")) {
		$config = Join-Path $PSScriptRoot $config.Substring(2)
}

if($IsWindows -eq $True)
{
    Write-Output "Running on Windows"
    $filePath = Join-Path $PSScriptRoot '/dist/win-x64/changeset.exe'
}

if($IsLinux -eq $True)
{
		Write-Output "Running on Linux"
    $filePath = Join-Path $PSScriptRoot '/dist/linux-x64/changeset'
    chmod +x $filePath
}

if($IsMacOS -eq $True)
{
		Write-Output "Running on MacOS"
    $filePath = Join-Path $PSScriptRoot '/dist/osx-x64/changeset'
    chmod +x $filePath
}

if ($filePath -eq $null) {
		Write-Output "Unsupported OS"
		exit 1
}

Write-Output "Running $filePath..."
$arguments = "-path=$path", "-config=$config", $pairs
Write-Output $arguments
$command = "$filePath $arguments"
Invoke-Expression $command

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}