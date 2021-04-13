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


$server = "https://www.nuget.org"
$readkey = Read-Host "默认服务器为nuget.org，确认按回车键`n如要切换为nuget.66soft.net，按 1`n如要切换为ncc.myget.org，按 2`n如要切换为osharp.myget.org，按3"
if ($readkey -eq 1) {
    $server = "http://nuget.66soft.net/nuget"
}
elseif ($readkey -eq 2) {
    $server = "https://www.myget.org/F/ncc/api/v2/package"
}
elseif ($readkey -eq 3) {
    $server = "https://www.myget.org/F/osharp/api/v2/package"
}

$rootPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
Write-Host ("当前目录：{0}" -f $rootPath)
$version = GetVersion
Write-Host ("当前版本：{0}" -f $version)

$nupkgs = ("{0}\nupkgs" -f $rootPath)
Write-Host $nupkgs
if(!(Test-Path $nupkgs))
{
    Write-Host ("输出文件夹 {0} 不存在" -f $nupkgs)
    exit
}

Set-Location $nupkgs
Write-Host ("`n正在查找nupkg发布包，当前目录：$(Get-Location)")

$files = [System.IO.Directory]::GetFiles($nupkgs, ("*.{0}*nupkg" -f $version))
Write-Host ("共找到 {0} 个版本号为 {1} 的nupkg文件" -f $files.Length, $version)
if($files.Length -eq 0){
    exit
}
Write-Host "是否继续发布？"

pause
$items = @()
foreach($file in $files){
    $obj = New-Object PSObject -Property @{
        Server = $server
        File = $file
    }
    
    $items += @($obj)
}

$items | ForEach-Object -Parallel {
    $item = $_
    $name = [System.IO.Path]::GetFileName($item.File)
    Write-Host ("正在 {0} 向发布{1}" -f $item.Server, $name)
    $server = @("push", $name) + @("-Source", $item.Server)
    & nuget $server
} -ThrottleLimit 5
pause