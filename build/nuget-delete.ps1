$version = Read-Host "请输入要删除的版本号"
$props = @(
"OSharpNS.Core", "OSharpNS.AspNetCore", "OSharpNS.Authorization.Datas", "OSharpNS.Authorization.Functions", "OSharpNS.AutoMapper",
"OSharpNS.EntityFrameworkCore", "OSharpNS.EntityFrameworkCore.MySql", "OSharpNS.EntityFrameworkCore.Oracle", 
"OSharpNS.EntityFrameworkCore.PostgreSql", "OSharpNS.EntityFrameworkCore.Sqlite", "OSharpNS.EntityFrameworkCore.SqlServer",
"OSharpNS.Exceptionless", "OSharpNS.Hangfire", "OSharpNS.Hosting.Apis", "OSharpNS.Hosting.Core", "OSharpNS.Hosting.EntityConfiguration",
"OSharpNS.Identity", "OSharpNS.Log4Net", "OSharpNS.MiniProfiler", "OSharpNS.Redis", "OSharpNS.Swagger", "OSharpNS", 
"OSharpNS.Template.Mvc_Angular", "OSharpNS.CodeGeneration"
)
foreach($prop in $props)
{
    nuget delete $prop $version -src "https://www.nuget.org" -NonInteractive
}