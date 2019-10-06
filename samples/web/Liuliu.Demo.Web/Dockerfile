FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
MAINTAINER LiuliuSoft i66soft@qq.com

WORKDIR /app
EXPOSE 80

COPY ./bin/Release/netcoreapp2.2/publish /app
ENTRYPOINT ["dotnet", "Liuliu.Demo.Web.dll"]