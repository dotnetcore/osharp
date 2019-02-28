## 使用方法
1. 到 [QQ互联](https://connect.qq.com/index.html) 申请应用，得到需要的`AppId`和`AppKey`。注意填写`网站回调域 `时，需要填写以`signin-qq`为路径的地址，例如主站为`https://www.osharp.org`，则填写`https://www.osharp.org/signin-qq`，也可以加一个`localhost`的测试域名，例如填写`https://www.osharp.org/signin-qq;http://localhost:4201/signin-qq`
2. 打开appsettings.json文件，添加如下代码

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

3. 在Startup.cs中的ConfigureServices方法中增加如下代码

```
services.AddAuthentication().AddQQ(qqOptions =>
{
    qqOptions.AppId = Configuration["Authentication:QQ:AppId"];
    qqOptions.AppKey = Configuration["Authentication:QQ:AppKey"];
});
```
4. 在 用户登录Controller中添加如下两个Action
点击登录页面的QQ图标时，用于向QQ服务器请求QQ登录页面
```
[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
public IActionResult ExternalLogin(string provider, string returnUrl = null)
{
    // Request a redirect to the external login provider.
    var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
    return Challenge(properties, provider);
}
```
QQ登录成功之后，获取到QQ用户信息时的回调，用于查找用户信息登录或者创建新用户
```
[HttpGet]
[AllowAnonymous]
public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
{
    if (remoteError != null)
    {
        ErrorMessage = $"Error from external provider: {remoteError}";
        return RedirectToAction(nameof(Login));
    }
    //获取QQ登录信息
    var info = await _signInManager.GetExternalLoginInfoAsync();
    if (info == null)
    {
        return RedirectToAction(nameof(Login));
    }
    //进行用户登录或者创建新用户的后续操作
    ...
}

```

## power by:
[https://github.com/china-live/QQConnect/tree/master/src/Microsoft.AspNetCore.Authentication.QQ](https://github.com/china-live/QQConnect/tree/master/src/Microsoft.AspNetCore.Authentication.QQ)