#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OSharp.Utils/OSharp.Utils.csproj", "src/OSharp.Utils/"]
COPY ["src/OSharp/OSharp.csproj", "src/OSharp/"]
COPY ["src/OSharp.AutoMapper/OSharp.AutoMapper.csproj", "src/OSharp.AutoMapper/"]
COPY ["src/OSharp.AspNetCore/OSharp.AspNetCore.csproj", "src/OSharp.AspNetCore/"]
COPY ["src/OSharp.EntityFrameworkCore/OSharp.EntityFrameworkCore.csproj", "src/OSharp.EntityFrameworkCore/"]
COPY ["src/OSharp.EntityFrameworkCore.MySql/OSharp.EntityFrameworkCore.MySql.csproj", "src/OSharp.EntityFrameworkCore.MySql/"]
COPY ["src/OSharp.EntityFrameworkCore.PostgreSql/OSharp.EntityFrameworkCore.PostgreSql.csproj", "src/OSharp.EntityFrameworkCore.PostgreSql/"]
COPY ["src/OSharp.EntityFrameworkCore.Sqlite/OSharp.EntityFrameworkCore.Sqlite.csproj", "src/OSharp.EntityFrameworkCore.Sqlite/"]
COPY ["src/OSharp.EntityFrameworkCore.SqlServer/OSharp.EntityFrameworkCore.SqlServer.csproj", "src/OSharp.EntityFrameworkCore.SqlServer/"]
COPY ["src/OSharp.Identity/OSharp.Identity.csproj", "src/OSharp.Identity/"]
COPY ["src/OSharp.Authorization.Functions/OSharp.Authorization.Functions.csproj", "src/OSharp.Authorization.Functions/"]
COPY ["src/OSharp.Authorization.Datas/OSharp.Authorization.Datas.csproj", "src/OSharp.Authorization.Datas/"]
COPY ["src/OSharp.Log4Net/OSharp.Log4Net.csproj", "src/OSharp.Log4Net/"]
COPY ["src/OSharp.MiniProfiler/OSharp.MiniProfiler.csproj", "src/OSharp.MiniProfiler/"]
COPY ["src/OSharp.Redis/OSharp.Redis.csproj", "src/OSharp.Redis/"]
COPY ["src/OSharp.Swagger/OSharp.Swagger.csproj", "src/OSharp.Swagger/"]
COPY ["src/OSharp.Hangfire/OSharp.Hangfire.csproj", "src/OSharp.Hangfire/"]
COPY ["src/OSharp.Hosting.Core/OSharp.Hosting.Core.csproj", "src/OSharp.Hosting.Core/"]
COPY ["src/OSharp.Hosting.EntityConfiguration/OSharp.Hosting.EntityConfiguration.csproj", "src/OSharp.Hosting.EntityConfiguration/"]
COPY ["src/OSharp.Hosting.Apis/OSharp.Hosting.Apis.csproj", "src/OSharp.Hosting.Apis/"]
COPY ["samples/web/Liuliu.Demo.WebApi/Liuliu.Demo.WebApi.csproj", "samples/web/Liuliu.Demo.WebApi/"]

RUN dotnet restore "./samples/web/Liuliu.Demo.WebApi/./Liuliu.Demo.WebApi.csproj"
COPY . .
WORKDIR "/src/samples/web/Liuliu.Demo.WebApi"
RUN dotnet build "./Liuliu.Demo.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Liuliu.Demo.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Liuliu.Demo.WebApi.dll"]