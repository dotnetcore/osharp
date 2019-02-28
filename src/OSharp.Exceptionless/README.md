# Exceptionless分布式异常日志模块

此模块用于记录异常信息到`Exceptionless`服务器（可以是官方服务器或者自部属服务器）中

## 使用方法

1. 通过nuget引用 `OSharp.Exceptionless` 程序集
> Install-Package OSharpNS.Exceptionless

2. 在 `appsettings.json` 中添加如下配置节点
```
{
  "OSharp": {
    "Exceptionless": {
      "ApiKey": "你的ApiKey",
      "ServerUrl": null,
      "Enabled": true
    } 
  }
}
```
配置说明：
 * ApiKey不能为空，
 * 如果使用官方服务器，`ServerUrl`保持为`null`即可，如果使用自部属的服务器，则改为服务器地址

3. 如果要禁用该模块，设置`Enabled: false`即可
