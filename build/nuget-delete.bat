:: �����ɾ��nuget�����ϵ�osharpָ���汾�İ�
@echo off
set /p version=������Ҫɾ���İ汾�ţ�
nuget delete OSharpNS.Core %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.AspNetCore %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Authorization.Datas %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Authorization.Functions %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.AutoMapper %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.EntityFrameworkCore %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.EntityFrameworkCore.MySql %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.EntityFrameworkCore.Oracle %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.EntityFrameworkCore.PostgreSql %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.EntityFrameworkCore.Sqlite %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.EntityFrameworkCore.SqlServer %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Exceptionless %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Hangfire %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Hosting.Apis %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Hosting.Core %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Hosting.EntityConfiguration %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Identity %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Log4Net %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.MiniProfiler %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Redis %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Swagger %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.Template.Mvc_Angular %version% -src https://www.nuget.org -NonInteractive
nuget delete OSharpNS.CodeGeneration %version% -src https://www.nuget.org -NonInteractive