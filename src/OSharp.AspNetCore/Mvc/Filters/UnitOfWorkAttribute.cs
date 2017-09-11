// -----------------------------------------------------------------------
//  <copyright file="UnitOfWorkAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-11 22:33</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Mvc.Filters;

using OSharp.Entity;


namespace OSharp.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 自动事务提交过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UnitOfWorkAttribute : ActionFilterAttribute
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 初始化一个<see cref="UnitOfWorkAttribute"/>类型的新实例
        /// </summary>
        public UnitOfWorkAttribute(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _unitOfWork.Commit();
        }
    }
}