// -----------------------------------------------------------------------
//  <copyright file="TestController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Liuliu.Demo.Identity;
using Liuliu.Demo.Identity.Dtos;
using Liuliu.Demo.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Collections;
using OSharp.Data;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-测试")]
    [ClassFilter]
    public class TestController : SiteApiControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IIdentityContract _identityContract;

        public TestController(UserManager<User> userManager, IIdentityContract identityContract)
        {
            _userManager = userManager;
            _identityContract = identityContract;
        }

        [HttpGet]
        [UnitOfWork]
        [MethodFilter]
        [Description("测试01")]
        public async Task<string> Test01()
        {
            List<object> list = new List<object>();

            if (!_userManager.Users.Any())
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

                OperationResult<User> result = await _identityContract.Register(dto);
                if (result.Succeeded)
                {
                    User user = result.Data;
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
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
                result = await _identityContract.Register(dto);
                if (result.Succeeded)
                {
                    User user = result.Data;
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
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
