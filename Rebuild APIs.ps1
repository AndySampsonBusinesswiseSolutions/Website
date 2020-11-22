$FileList = Get-ChildItem -Path C:\wamp64\www\Website\Code\ -Recurse -Filter *.csproj -Force -ErrorAction SilentlyContinue | Select-Object FullName
Foreach ($File in $FileList) {
    dotnet.exe build $File.FullName /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary
}