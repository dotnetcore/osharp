function GetVersion()
{
    $file = "version.props"
    $xml = New-Object -TypeName XML
    $xml.Load($file)
    $version = $xml.Project.PropertyGroup.Version
    if($version.contains("VersionSuffixVersion"))
    {
        $version = "{0}.{1}{2}{3}" -f $xml.Project.PropertyGroup.VersionMain,$xml.Project.PropertyGroup.VersionPrefix,$xml.Project.PropertyGroup.VersionSuffix,$xml.Project.PropertyGroup.VersionSuffixVersion
    }
    else
    {
        $version = "{0}.{1}" -f $xml.Project.PropertyGroup.VersionMain,$xml.Project.PropertyGroup.VersionPrefix
    }
    return $version
}

function SetOsharpNSVersion()
{
    $file = "OSharpNS.nuspec"
    $xml = New-Object -TypeName XML
    $xml.Load($file)
    $xml.package.metadata.version = $version
    #$nodes = $xml.SelectNodes("/package/metadata/dependencies/group")
    $nodes = $xml.package.metadata.dependencies.group.dependency
    foreach($node in $nodes)
    {
        $node.version = $version
    }
    # Utf-8 with BOM 保存
    $encoding = New-Object System.Text.UTF8Encoding($true)
    $writer = New-Object System.IO.StreamWriter($file, $false, $encoding)
    $xml.Save($writer)
    $writer.Close()
    Write-Host ("{0} 更新成功，新版本：{1}`n" -f $file,$version)
}

$rootPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
Write-Host ("当前目录：{0}" -f $rootPath)
$version = GetVersion
Write-Host ("当前版本：{0}" -f $version)
SetOsharpNSVersion


$nupkgs = ".\nupkgs"
if(Test-Path $nupkgs)
{
    Remove-Item ("{0}\*.*" -f $nupkgs)
    Write-Host ("清空 {0} 文件夹" -f $nupkgs)
}
else
{
    New-Item -Path . -Name $nupkgs -ItemType "directory" -Force
    Write-Host ("创建 {0} 文件夹" -f $nupkgs)
}
$props = @("OSharp", "OSharp.AspNetCore", "OSharp.Authorization.Datas", "OSharp.Authorization.Functions", 
"OSharp.AutoMapper", "OSharp.EntityFrameworkCore","OSharp.EntityFrameworkCore.MySql", "OSharp.EntityFrameworkCore.Oracle", 
"OSharp.EntityFrameworkCore.PostgreSql", "OSharp.EntityFrameworkCore.Sqlite","OSharp.EntityFrameworkCore.SqlServer", 
"OSharp.Exceptionless", "OSharp.Hangfire", "OSharp.Hosting.Apis", "OSharp.Hosting.Core", "OSharp.Hosting.EntityConfiguration", 
"OSharp.Identity", "OSharp.Log4Net", "OSharp.MiniProfiler", "OSharp.Redis", "OSharp.Swagger", "OSharp.Wpf")
foreach($prop in $props)
{
    $path = ("../src/{0}/{0}.csproj" -f $prop)
    dotnet build $path -c Release
    dotnet pack $path -c Release --output $nupkgs
}

nuget pack .\OSharpNS.nuspec -OutputDirectory $nupkgs
Invoke-Item $nupkgs
pause