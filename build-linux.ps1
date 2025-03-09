# Call Sample
# sed -i 's/\r//g' build-linux.ps1 && pwsh -File build-linux.ps1

dotnet publish src/ChangeSet/ChangeSet.csproj -c Release -o dist/linux-x64/ -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true /p:DebugType=None /p:IncludeNativeLibrariesForSelfExtract=true

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}
