## 使用方法
1. 打开appsettings.json文件，添加如下代码

```
{
    "Authentication": {
        "QQ": {
            "AppId": "你申请的QQ互联AppID",
            "AppKey": "你申请的QQ互联AppKey"
        }
    },
    //省略....
}
```

2. 在Startup.cs中的ConfigureServices方法中增加如下代码

```
services.AddAuthentication().AddQQ(qqOptions =>
{
    qqOptions.AppId = Configuration["Authentication:QQ:AppId"];
    qqOptions.AppKey = Configuration["Authentication:QQ:AppKey"];
});
```

## power by:
[https://github.com/china-live/QQConnect/tree/master/src/Microsoft.AspNetCore.Authentication.QQ](https://github.com/china-live/QQConnect/tree/master/src/Microsoft.AspNetCore.Authentication.QQ)