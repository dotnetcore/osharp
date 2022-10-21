#Requires -Version 6

function WriteXml([System.Xml.XmlDocument]$xml, [string]$file)
{
    $encoding = New-Object System.Text.UTF8Encoding($true)
    $writer = New-Object System.IO.StreamWriter($file, $false, $encoding)
    $xml.Save($writer)
    $writer.Close()
}

function GetVersion()
{
    $file = "$($rootPath)\build\version.props"
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
    $file = "$($rootPath)\build\OSharpNS.nuspec"
    Write-Host ("正在更新文件 $($file) 的版本号：$($version)")
    $xml = New-Object -TypeName XML
    $xml.Load($file)
    $xml.package.metadata.version = $version
    $nodes = $xml.package.metadata.dependencies.group.dependency
    foreach($node in $nodes)
    {
        $node.version = $version
    }
    WriteXml $xml $file
    Write-Host "OSharp.nuspec 版本号更新成功"
}

function BuildNugetPackages()
{
    $output = "$($rootPath)\build\output"
    if(Test-Path $output)
    {
        Remove-Item ("$($output)\*.*")
        Write-Host ("清空文件夹：$($output)")
    }
    else
    {
        New-Item -ItemType directory -Path $output
        Write-Host "创建文件夹：$($output)"
    }

    $projs = @(
    "OSharp.Utils",
    "OSharp",
    "OSharp.AspNetCore",
    "OSharp.AutoMapper",
    "OSharp.Exceptionless",
    "OSharp.Hangfire",
    "OSharp.Log4Net",
    "OSharp.MiniProfiler",
    "OSharp.Redis",
    "OSharp.Swagger",
    "OSharp.Wpf",
    "OSharp.EntityFrameworkCore",
    "OSharp.EntityFrameworkCore.MySql",
    "OSharp.EntityFrameworkCore.Oracle", 
    "OSharp.EntityFrameworkCore.PostgreSql",
    "OSharp.EntityFrameworkCore.Sqlite",
    "OSharp.EntityFrameworkCore.SqlServer", 
    "OSharp.Identity",
    "OSharp.Authorization.Datas",
    "OSharp.Authorization.Functions",
    "OSharp.Hosting.Core",
    "OSharp.Hosting.EntityConfiguration", 
    "OSharp.Hosting.Apis"
    )
    foreach($proj in $projs)
    {
        $path = "$($rootPath)/src/$($proj)/$($proj).csproj"
        Write-Host "`n正在编译：$($proj)"
        dotnet build $path -c Release
        Write-Host "`n正在打包：$($proj)"
        dotnet pack $path -c Release --output $output
    }

    $file = "$($rootPath)\build\OSharpNS.nuspec"
    $nuget = "D:\GreenSoft\Envs\nuget\nuget.exe"
    & $nuget pack $file -OutputDirectory $output
    if($ENV:WORKSPACE -eq $null)
    {
        Invoke-Item $output
    }
}

function PushNugetPackages()
{
    $output = "$($rootPath)\build\output"
    if(!(Test-Path $output))
    {
        Write-Host "输出文件夹 $($output) 不存在"
        exit    
    }
    Write-Host "正在查找 nupkg 发布包"
    $files = [System.IO.Directory]::GetFiles($output, "*.$($version)*nupkg")
    Write-Host "共找到 $($files.Length) 个版本号为 $($version) 的nuget文件"
    if($files.Length -eq 0)
    {
        exit
    }
    
    $key = "D:\GreenSoft\Envs\nuget\nuget.org-apikey.txt"
    $key = [System.IO.File]::ReadAllText($key)
    $server = "https://api.nuget.org/v3/index.json"
    Write-Host "nuget服务器：$($server)，密钥：$($key)"
    $items=@()
    foreach($file in $files)
    {
        $obj = New-Object PSObject -Property @{
            Server = $server
            File = $file
            Key = $key
        }
        $items += @($obj)
    }

    $items | ForEach-Object -Parallel {
        $nuget = "D:\GreenSoft\Envs\nuget\nuget.exe"
        $item = $_
        $name = [System.IO.Path]::GetFileName($item.File)
        Write-Host ("正在 {0} 向发布{1}" -f $item.Server, $name)
        $server = @("push", $item.File, "-Source", $item.Server, "-ApiKey", $item.Key, "-SkipDuplicate")
        & $nuget $server
    } -ThrottleLimit 5
}

$now = [DateTime]::Now
$rootPath = ($ENV:WORKSPACE)
if($rootPath -eq $null)
{
    $rootPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
    $rootPath = Split-Path -Parent $rootPath
}
Write-Host ("当前目录：$($rootPath)")
$version = GetVersion
Write-Host ("当前版本：$($version)")
SetOsharpNSVersion
BuildNugetPackages
#PushNugetPackages
