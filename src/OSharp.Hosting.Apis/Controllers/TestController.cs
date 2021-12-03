// -----------------------------------------------------------------------
//  <copyright file="TestController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Hosting.Identity;
using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;
using OSharp.Hosting.Systems;
using OSharp.Hosting.Systems.Dtos;


namespace OSharp.Hosting.Apis.Controllers
{
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

        private ISystemsContract SystemsContract => _provider.GetRequiredService<ISystemsContract>();

        [HttpGet]
        [Description("测试")]
        public async Task<string> Test()
        {
            var thread = new Thread(testMongoDB);
            thread.Start();
            var thread1 = new Thread(testMongoDB);
            //var thread2 = new Thread(testMongoDB);
            //var thread3 = new Thread(testMongoDB);
            thread1.Start();
            //thread2.Start();
            //thread3.Start();

            return "OK";
        }

        [HttpGet]
        [Description("测试线程")]
        public static async void testMongoDB()
        {
            for (int i = 0; i < 100; i++)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        string html = await client.GetStringAsync("http://localhost:7001/api/Test/CreateMenu");
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }

        [HttpGet]
        [UnitOfWork]
        [Description("创建菜单")]
        public async Task<string> CreateMenu()
        {
            var list = new List<MenuInputDto>();
            var dto = new MenuInputDto()
            {
                Acl = "Acl",
                Data = "Data",
                Icon = "Icon",
                IsEnabled = true,
                IsSystem = true,
                Name = Guid.NewGuid().ToString(),
                OrderCode = 0.00,
                Target = "http://www.baidu.com",
                Text = "Text",
                TreePathString = "TreePathString",
                Url = "http://www.baidu.com"
            };
            list.Add(dto);
            await SystemsContract.CreateMenuInfos(list.ToArray());
            return "OK";
        }
        
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
            var ex = context.Exception.Message;
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
}
