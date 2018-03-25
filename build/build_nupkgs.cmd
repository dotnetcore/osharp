cd ..\
dotnet restore
dotnet publish osharp-ns20.sln --configuration Release
dotnet pack --output %cd%\nupkgs --configuration Release
pause