// -----------------------------------------------------------------------
//  <copyright file="AuditOperationAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-01 18:57</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Audits;
using OSharp.Core.Functions;
using OSharp.Dependency;
using OSharp.Secutiry.Claims;


namespace OSharp.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 操作审计拦截器，负责发起并记录功能的操作日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuditOperationAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IFunction function = context.GetExecuteFunction();
            if (function == null || !function.AuditOperationEnabled)
            {
                return;
            }

            ScopedDictionary dict = context.HttpContext.RequestServices.GetService<ScopedDictionary>();
            dict.Function = function;

            AuditOperation operation = new AuditOperation
            {
                FunctionName = function.Name,
                Ip = context.HttpContext.GetClientIp(),
                UserAgent = context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault(),
                CreatedTime = DateTime.Now
            };
            if (context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.User.Identity is ClaimsIdentity identity)
            {
                operation.UserId = identity.GetUserId();
                operation.UserName = identity.GetUserName();
                operation.NickName = identity.GetNickName();
            }

            dict.AuditOperation = operation;
        }

        /// <inheritdoc />
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        /// <inheritdoc />
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            IServiceProvider provider = context.HttpContext.RequestServices;
            ScopedDictionary dict = provider.GetService<ScopedDictionary>();
            if (dict.AuditOperation?.FunctionName == null)
            {
                return;
            }
            IAuditStore store = provider.GetService<IAuditStore>();
            store?.Save(dict.AuditOperation);
        }
    }
}

/* Filter执行顺序
ClassFilter - OnActionExecuting
MethodFilter - OnActionExecuting
MethodFilter - OnActionExecuted
ClassFilter - OnActionExecuted
ClassFilter - OnResultExecuting
MethodFilter - OnResultExecuting
MethodFilter - OnResultExecuted
ClassFilter - OnResultExecuted
*/
