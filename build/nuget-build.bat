del /f /s /q nupkgs\*.*

dotnet build ../osharp.sln -c Release

dotnet pack ../src/OSharp/OSharp.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.AspNetCore/OSharp.AspNetCore.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Authorization.Datas/OSharp.Authorization.Datas.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Authorization.Functions/OSharp.Authorization.Functions.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.AutoMapper/OSharp.AutoMapper.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.EntityFrameworkCore/OSharp.EntityFrameworkCore.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.EntityFrameworkCore.MySql/OSharp.EntityFrameworkCore.MySql.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.EntityFrameworkCore.Oracle/OSharp.EntityFrameworkCore.Oracle.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.EntityFrameworkCore.PostgreSql/OSharp.EntityFrameworkCore.PostgreSql.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.EntityFrameworkCore.Sqlite/OSharp.EntityFrameworkCore.Sqlite.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.EntityFrameworkCore.SqlServer/OSharp.EntityFrameworkCore.SqlServer.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Exceptionless/OSharp.Exceptionless.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Hangfire/OSharp.Hangfire.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Hosting.Apis/OSharp.Hosting.Apis.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Hosting.Core/OSharp.Hosting.Core.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Hosting.EntityConfiguration/OSharp.Hosting.EntityConfiguration.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Identity/OSharp.Identity.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Log4Net/OSharp.Log4Net.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.MiniProfiler/OSharp.MiniProfiler.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Redis/OSharp.Redis.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Swagger/OSharp.Swagger.csproj -c Release --output nupkgs
dotnet pack ../src/OSharp.Wpf/OSharp.Wpf.csproj -c Release --output nupkgs

nuget pack ./osharpns.nuspec -OutputDirectory nupkgs

start "" nupkgs
exit