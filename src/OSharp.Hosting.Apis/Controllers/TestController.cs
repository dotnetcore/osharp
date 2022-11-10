// -----------------------------------------------------------------------
//  <copyright file="TestController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using OSharp.Hosting.Identity;
using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;


namespace OSharp.Hosting.Apis.Controllers;

[Description("网站-测试")]
[ClassFilter]
public class TestController : SiteApiControllerBase
{
    private readonly IServiceProvider _provider;

    public TestController(IServiceProvider provider)
        : base(provider)
    {
        _provider = provider;
    }

    private UserManager<User> UserManager => _provider.GetRequiredService<UserManager<User>>();

    private IIdentityContract IdentityContract => _provider.GetRequiredService<IIdentityContract>();

    [HttpGet]
    [UnitOfWork]
    [MethodFilter]
    [Description("测试01")]
    public async Task<string> Test01()
    {
        List<object> list = new List<object>();

        if (!UserManager.Users.Any())
        {
            RegisterDto dto = new RegisterDto
            {
                UserName = "admin",
                Password = "osharp123456",
                ConfirmPassword = "osharp123456",
                Email = "i66soft@qq.com",
                NickName = "大站长",
                RegisterIp = HttpContext.GetClientIp()
            };

            OperationResult<User> result = await IdentityContract.Register(dto);
            if (result.Succeeded)
            {
                User user = result.Data;
                user.EmailConfirmed = true;
                await UserManager.UpdateAsync(user);
            }
            list.Add(result.Message);

            dto = new RegisterDto()
            {
                UserName = "osharp",
                Password = "osharp123456",
                Email = "mf.guo@qq.com",
                NickName = "测试号",
                RegisterIp = HttpContext.GetClientIp()
            };
            result = await IdentityContract.Register(dto);
            if (result.Succeeded)
            {
                User user = result.Data;
                user.EmailConfirmed = true;
                await UserManager.UpdateAsync(user);
            }
            list.Add(result.Message);
        }

        return list.ExpandAndToString("\r\n");
    }
        
    [HttpPost]
    [Description("测试2")]
    [RoleLimit]
    public async Task<int> Test02()
    {
        IServiceProvider provider = HttpContext.RequestServices;
        string token = Guid.NewGuid().ToString("N").Substring(0, 5);
        RoleManager<Role> manager = provider.GetRequiredService<RoleManager<Role>>();
        await manager.CreateAsync(new Role() { Name = "测试角色3" + token, Remark = "测试角色001描述" });
        await manager.CreateAsync(new Role() { Name = "测试角色4" + token, Remark = "测试角色002描述" });

        return provider.GetRequiredService<RoleManager<Role>>().Roles.Count();
    }
}

public class ClassFilter : ActionFilterAttribute, IExceptionFilter
{
    private ILogger _logger;

    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _logger = context.HttpContext.RequestServices.GetLogger<ClassFilter>();
        _logger.LogInformation("ClassFilter - OnActionExecuting");
    }

    /// <inheritdoc />
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("ClassFilter - OnActionExecuted");
    }

    /// <inheritdoc />
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        _logger.LogInformation("ClassFilter - OnResultExecuting");
    }

    /// <inheritdoc />
    public override void OnResultExecuted(ResultExecutedContext context)
    {
        _logger.LogInformation("ClassFilter - OnResultExecuted");
    }

    /// <summary>
    /// Called after an action has thrown an <see cref="T:System.Exception" />.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ExceptionContext" />.</param>
    public void OnException(ExceptionContext context)
    {
        _ = context.Exception.Message;
        _logger.LogInformation("ClassFilter - OnException");
    }
}

public class MethodFilter : ActionFilterAttribute
{
    private ILogger _logger;

    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _logger = context.HttpContext.RequestServices.GetLogger<MethodFilter>();
        _logger.LogInformation("MethodFilter - OnActionExecuting");
    }

    /// <inheritdoc />
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("MethodFilter - OnActionExecuted");
    }

    /// <inheritdoc />
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        _logger.LogInformation("MethodFilter - OnResultExecuting");
    }

    /// <inheritdoc />
    public override void OnResultExecuted(ResultExecutedContext context)
    {
        _logger.LogInformation("MethodFilter - OnResultExecuted");
    }

}
