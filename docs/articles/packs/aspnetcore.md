# AspNetCore模块：AspNetCorePack

* 级别：PackLevel.Core
* 启动顺序：2
* 位置：OSharp.AspNetCore.dll

> - [为框架提供统一初始化入口](#01)
> - [为服务定位模式提供HttpRequest的Scoped服务解析](#02)
> - [提供一个基于缓存的验证码存储/验证处理器](#03)
> - [定义了一个Ajax操作结果类，用于返回Ajax操作结果](#04)
> - [定义了一些常用中间件](#05)

---
AspNetCore模块是AspNetCore常用功能的封装，主要职责如下：

### <a id="01"/>为框架提供统一初始化入口 `app.UseOSharp`

调用模块管理器`OSharpPackManager`，执行框架各个模块`UsePack`方法，对Pack进行初始化操作。
```
/// <summary>
/// OSharp框架初始化
/// </summary>
public static IApplicationBuilder UseOSharp(this IApplicationBuilder app)
{
    IServiceProvider provider = app.ApplicationServices;
    OSharpPackManager packManager = provider.GetService<OSharpPackManager>();
    packManager.UsePacks(app);
    
    return app;
}
```
在启动类`Startup.cs`中，入口调用代码如下：
```
public void Configure(IApplicationBuilder app)
{
    // ...
    app.UseOSharp();
    // ...
}
```

### <a id="02"/>为服务定位模式`ServiceLocator`提供`HttpRequest`的Scoped服务解析

通过注入`IHttpContextAccessor`获得`HttpContext`对象，将服务定位器`ServiceLocator`中的服务解析转嫁到`HttpContextAccessor.HttpContext.RequestServices`来解析。
```
/// <summary>
/// Request的<see cref="ServiceLifetime.Scoped"/>服务解析器
/// </summary>
public class RequestScopedServiceResolver : IScopedServiceResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 初始化一个<see cref="RequestScopedServiceResolver"/>类型的新实例
    /// </summary>
    public RequestScopedServiceResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 获取 是否可解析
    /// </summary>
    public bool ResolveEnabled => _httpContextAccessor.HttpContext != null;

    /// <summary>
    /// 获取 <see cref="ServiceLifetime.Scoped"/>生命周期的服务提供者
    /// </summary>
    public IServiceProvider ScopedProvider
    {
        get { return _httpContextAccessor.HttpContext.RequestServices; }
    }

    /// <summary>
    /// 获取指定服务类型的实例
    /// </summary>
    /// <typeparam name="T">服务类型</typeparam>
    /// <returns></returns>
    public T GetService<T>()
    {
        return _httpContextAccessor.HttpContext.RequestServices.GetService<T>();
    }

    /// <summary>
    /// 获取指定服务类型的实例
    /// </summary>
    /// <param name="serviceType">服务类型</param>
    /// <returns></returns>
    public object GetService(Type serviceType)
    {
        return _httpContextAccessor.HttpContext.RequestServices.GetService(serviceType);
    }

    /// <summary>
    /// 获取指定服务类型的所有实例
    /// </summary>
    /// <typeparam name="T">服务类型</typeparam>
    /// <returns></returns>
    public IEnumerable<T> GetServices<T>()
    {
        return _httpContextAccessor.HttpContext.RequestServices.GetServices<T>();
    }

    /// <summary>
    /// 获取指定服务类型的所有实例
    /// </summary>
    /// <param name="serviceType">服务类型</param>
    /// <returns></returns>
    public IEnumerable<object> GetServices(Type serviceType)
    {
        return _httpContextAccessor.HttpContext.RequestServices.GetServices(serviceType);
    }
}
```
在AspNetCorePack模块初始化时，注入单例服务`IScopedServiceResolver`
```
services.AddSingleton<IScopedServiceResolver, RequestScopedServiceResolver>();
```
如此，在服务定位器`ServiceLocator`中，即可获取`IScopedServiceResolver`实例进行服务对象的解析工作。

### <a id="03"/>提供一个基于缓存的验证码存储/验证处理器`VerifyCodeHandler`

提供验证码服务时，需要服务端进行存储，通常使用`Session`服务来实现，但为了一个验证码而开启Session，总让人感觉不爽。于是框架提供了一个基于缓存`IDistributedCache`的验证码存储/验证实现。流程如下：
* 生成验证码文本后，存储时生成一个缓存key，返回验证码时同时返回这个key
* 将验证码图片和缓存key序列化成base64字符串返回给前端
* 前端接收到base64字符串，解析出图片和缓存key，显示图片，提交验证码时同时提交缓存key
* 后端接收到缓存key后，从缓存中取出数据与提交的验证码文本进行对比验证，验证之后清除缓存
```
/// <summary>
/// 验证码处理类
/// </summary>
public static class VerifyCodeHandler
{
    private const string Separator = "#$#";

    /// <summary>
    /// 校验验证码有效性
    /// </summary>
    /// <param name="code">要校验的验证码</param>
    /// <param name="id">验证码编号</param>
    /// <param name="removeIfSuccess">验证成功时是否移除</param>
    /// <returns></returns>
    public static bool CheckCode(string code, string id, bool removeIfSuccess = true)
    {
        if (string.IsNullOrEmpty(code))
        {
            return false;
        }
        string key = $"{OsharpConstants.VerifyCodeKeyPrefix}_{id}";
        IDistributedCache cache = ServiceLocator.Instance.GetService<IDistributedCache>();
        bool flag = code.Equals(cache.GetString(key), StringComparison.OrdinalIgnoreCase);
        if (removeIfSuccess && flag)
        {
            cache.Remove(key);
        }
        return flag;
    }

    /// <summary>
    /// 设置验证码到Session中
    /// </summary>
    public static void SetCode(string code, out string id)
    {
        id = Guid.NewGuid().ToString("N");
        string key = $"{OsharpConstants.VerifyCodeKeyPrefix}_{id}";
        IDistributedCache cache = ServiceLocator.Instance.GetService<IDistributedCache>();
        const int seconds = 60 * 3;
        cache.SetString(key, code, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds) });
    }

    /// <summary>
    /// 将图片序列化成字符串
    /// </summary>
    public static string GetImageString(Image image, string id)
    {
        Check.NotNull(image, nameof(image));
        using (MemoryStream ms = new MemoryStream())
        {
            image.Save(ms, ImageFormat.Png);
            byte[] bytes = ms.ToArray();
            string str = $"data:image/png;base64,{bytes.ToBase64String()}{Separator}{id}";
            return str.ToBase64String();
        }
    }
}
```

### <a id="04"/>定义了一个Ajax操作结果类，用于返回Ajax操作结果

Ajax操作结果类`AjaxResult`封装了 操作结果类型`AjaxResultType`(消息/成功/错误/未登录/权限不足/未找到/资源锁定)、提示文本消息、附加数据 等信息。前端JS代码接收到此消息体进行不同操作类型的结果处理。
```
/// <summary>
/// 表示Ajax操作结果 
/// </summary>
public class AjaxResult
{
    /// <summary>
    /// 获取或设置 Ajax操作结果类型
    /// </summary>
    public AjaxResultType Type { get; set; }

    /// <summary>
    /// 获取或设置 消息内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 获取或设置 返回数据
    /// </summary>
    public object Data { get; set; }
}

/// <summary>
/// 表示 ajax 操作结果类型的枚举
/// </summary>
public enum AjaxResultType
{
    /// <summary>
    /// 消息结果类型
    /// </summary>
    Info = 203,

    /// <summary>
    /// 成功结果类型
    /// </summary>
    Success = 200,

    /// <summary>
    /// 异常结果类型
    /// </summary>
    Error = 500,

    /// <summary>
    /// 用户未登录
    /// </summary>
    UnAuth = 401,

    /// <summary>
    /// 已登录，但权限不足
    /// </summary>
    Forbidden = 403,

    /// <summary>
    /// 资源未找到
    /// </summary>
    NoFound = 404,

    /// <summary>
    /// 资源被锁定
    /// </summary>
    Locked = 423
}
```

### <a id="05"/>定义了一些常用中间件

* `NodeExceptionHandlerMiddleware`：JS操作结果处理中间件，将JS请求的Response封装成`AjaxResult`进行返回
* `NodeNoFoundHandlerMiddleware`：JS操作中如果遇到 404，将重定身到 `index.html` 页中，以符合前端框架的需求
