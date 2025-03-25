#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# 安装curl用于健康检查
# RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
RUN apt-get update && apt install -y curl sqlite3 libsqlite3-dev && rm -rf /var/lib/apt/lists/*
#USER app
EXPOSE 80
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release 
WORKDIR /src

# 复制项目文件，优化缓存
COPY ["samples/web/Liuliu.Demo.WebApi/Liuliu.Demo.WebApi.csproj", "samples/web/Liuliu.Demo.WebApi/"]
COPY ["src/OSharp.AutoMapper/OSharp.AutoMapper.csproj", "src/OSharp.AutoMapper/"]
COPY ["src/OSharp.EntityFrameworkCore.MySql/OSharp.EntityFrameworkCore.MySql.csproj", "src/OSharp.EntityFrameworkCore.MySql/"]
COPY ["src/OSharp.EntityFrameworkCore.Oracle/OSharp.EntityFrameworkCore.Oracle.csproj", "src/OSharp.EntityFrameworkCore.Oracle/"]
COPY ["src/OSharp.EntityFrameworkCore.PostgreSql/OSharp.EntityFrameworkCore.PostgreSql.csproj", "src/OSharp.EntityFrameworkCore.PostgreSql/"]
COPY ["src/OSharp.EntityFrameworkCore.Sqlite/OSharp.EntityFrameworkCore.Sqlite.csproj", "src/OSharp.EntityFrameworkCore.Sqlite/"]
COPY ["src/OSharp.EntityFrameworkCore.SqlServer/OSharp.EntityFrameworkCore.SqlServer.csproj", "src/OSharp.EntityFrameworkCore.SqlServer/"]
COPY ["src/OSharp.Hangfire/OSharp.Hangfire.csproj", "src/OSharp.Hangfire/"]
COPY ["src/OSharp.Hosting.Apis/OSharp.Hosting.Apis.csproj", "src/OSharp.Hosting.Apis/"]
COPY ["src/OSharp.Log4Net/OSharp.Log4Net.csproj", "src/OSharp.Log4Net/"]
COPY ["src/OSharp.MiniProfiler/OSharp.MiniProfiler.csproj", "src/OSharp.MiniProfiler/"]
COPY ["src/OSharp.Redis/OSharp.Redis.csproj", "src/OSharp.Redis/"]
COPY ["src/OSharp.Swagger/OSharp.Swagger.csproj", "src/OSharp.Swagger/"]
# 这些部分依赖上面的项目，后续复制缓存优化
COPY ["src/OSharp.Utils/OSharp.Utils.csproj", "src/OSharp.Utils/"]
COPY ["src/OSharp/OSharp.csproj", "src/OSharp/"]
COPY ["src/OSharp.AspNetCore/OSharp.AspNetCore.csproj", "src/OSharp.AspNetCore/"]
COPY ["src/OSharp.EntityFrameworkCore/OSharp.EntityFrameworkCore.csproj", "src/OSharp.EntityFrameworkCore/"]
COPY ["src/OSharp.Identity/OSharp.Identity.csproj", "src/OSharp.Identity/"]
COPY ["src/OSharp.Authorization.Functions/OSharp.Authorization.Functions.csproj", "src/OSharp.Authorization.Functions/"]
COPY ["src/OSharp.Authorization.Datas/OSharp.Authorization.Datas.csproj", "src/OSharp.Authorization.Datas/"]
COPY ["src/OSharp.Hosting.Core/OSharp.Hosting.Core.csproj", "src/OSharp.Hosting.Core/"]
COPY ["src/OSharp.Hosting.EntityConfiguration/OSharp.Hosting.EntityConfiguration.csproj", "src/OSharp.Hosting.EntityConfiguration/"]

# 先恢复依赖，利用Docker缓存
RUN dotnet restore "samples/web/Liuliu.Demo.WebApi/Liuliu.Demo.WebApi.csproj"

# 然后复制所有源代码
COPY . .
WORKDIR "/src/samples/web/Liuliu.Demo.WebApi"
RUN dotnet build "Liuliu.Demo.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release 
RUN dotnet publish "Liuliu.Demo.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# 添加健康检查
HEALTHCHECK --interval=30s --timeout=30s --start-period=5s --retries=3 CMD curl --fail http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "Liuliu.Demo.WebApi.dll"]