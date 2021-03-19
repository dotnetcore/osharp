// -----------------------------------------------------------------------
//  <copyright file="TransactionAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-09-07 22:01</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using AspectCore.DynamicProxy;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Exceptions;


namespace OSharp.Entity.DynamicProxy
{
    /// <summary>
    /// 事务拦截器，根据操作结果来决定是否自动提交事务
    /// </summary>
    public sealed class TransactionalAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 获取或设置 是否判断返回值的成功或失败
        /// </summary>
        public bool RequiredCheckReturnValue { get; set; } = true;

        /// <summary>执行拦截业务，并根据返回结果决定提交事务还是回滚</summary>
        /// <param name="context">切面上下文</param>
        /// <param name="next">被拦截并包装的操作</param>
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            IServiceProvider provider = context.ServiceProvider;
            IUnitOfWork unitOfWork = provider.GetUnitOfWork(true);
            try
            {
                await next(context);
                if (!RequiredCheckReturnValue)
                {
#if NET5_0
                    await unitOfWork.CommitAsync();
#else
                    unitOfWork.Commit();
#endif
                    return;
                }
                Type returnType = context.ProxyMethod.ReturnType;
                ITransactionDecision decision = provider.GetServices<ITransactionDecision>()
                    .FirstOrDefault(m => m.IsFit(returnType));
                if (decision == null)
                {
                    throw new OsharpException($"无法找到与结果类型 {returnType} 匹配的 {typeof(ITransactionDecision)} 事务裁决器，请继承接口实现一个");
                }
                if (decision.CanCommit(context.ReturnValue))
                {
#if NET5_0
                    await unitOfWork.CommitAsync();
#else
                    unitOfWork.Commit();
#endif
                }
            }
            catch (Exception)
            {
#if NET5_0
                    await unitOfWork.CommitAsync();
#else
                unitOfWork.Commit();
#endif
                throw;
            }
        }
    }
}