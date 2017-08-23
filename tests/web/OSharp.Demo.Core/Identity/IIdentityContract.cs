// -----------------------------------------------------------------------
//  <copyright file="IIdentityContract.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 14:11</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

using OSharp.Demo.Identity.Entities;
using OSharp.Dependency;
using OSharp.Entity;


namespace OSharp.Demo.Identity
{
    /// <summary>
    /// 业务契约：身份认证模块
    /// </summary>
    public interface IIdentityContract : IScopeDependency
    {
        #region 用户业务

        /// <summary>
        /// 获取用户查询数据集
        /// </summary>
        /// <param name="propertySelectors">要Include的属性表达式</param>
        /// <returns></returns>
        IQueryable<User> Users(params Expression<Func<User, object>>[] propertySelectors);

        #endregion
    }
}