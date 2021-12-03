# Swagger 模块

## 用法
可按照如下配置方式使用：
1. 通过nuget引用 `OSharp.Swagger` 程序集
> Install-Package OSharpNS.Swagger
2. 在 `appsettings.json` 中添加如下配置节点
```
"Swagger": {
  "Endpoints": [
    {
      "Title": "框架API",
      "Version": "v1",
      "Url": "/swagger/v1/swagger.json"
    },
    {
      "Title": "业务API",
      "Version": "buss",
      "Url": "/swagger/buss/swagger.json"
    }
  ],
  "RoutePrefix": "swagger",
  "IsHideSchemas": false,
  "MiniProfiler": true,
  "Enabled": true
}
```
3. 要禁用Swagger，可以设置`Enabled: false`
