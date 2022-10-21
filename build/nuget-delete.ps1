$version = Read-Host "请输入要删除的版本号"
$props = @(
"OSharp.Core", "OSharp.AspNetCore", "OSharp.Authorization.Datas", "OSharp.Authorization.Functions", "OSharp.AutoMapper",
"OSharp.EntityFrameworkCore", "OSharp.EntityFrameworkCore.MySql", "OSharp.EntityFrameworkCore.Oracle", 
"OSharp.EntityFrameworkCore.PostgreSql", "OSharp.EntityFrameworkCore.Sqlite", "OSharp.EntityFrameworkCore.SqlServer",
"OSharp.Exceptionless", "OSharp.Hangfire", "OSharp.Hosting.Apis", "OSharp.Hosting.Core", "OSharp.Hosting.EntityConfiguration",
"OSharp.Identity", "OSharp.Log4Net", "OSharp.MiniProfiler", "OSharp.Redis", "OSharp.Swagger", "OSharp.Utils", "OSharp.Wpf", "OSharp", 
"OSharp.Template.Mvc_Angular", "OSharp.CodeGeneration"
)
foreach($prop in $props)
{
    nuget delete $prop $version -src "https://www.nuget.org" -NonInteractive
    # nuget delete $prop $version -Source "http://nuget.66soft.net/nuget" -NonInteractive
}
