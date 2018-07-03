// -----------------------------------------------------------------------
//  <copyright file="CommonService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-03 12:59</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

using OSharp.Collections;
using OSharp.Dependency;
using OSharp.Json;


namespace Liuliu.Demo.Common
{
    /// <summary>
    /// 业务实现：通用业务
    /// </summary>
    public class CommonService : ICommonContract, IScopeDependency
    {
        /// <summary>
        /// 测试测试
        /// </summary>
        public string Test()
        {
            List<object> list = new List<object>();

            ClaimsPrincipal user = ServiceLocator.Instance.GetCurrentUser();
            list.Add(user == null);
            list.Add(user?.GetType());
            list.Add(user?.Identity.Name);
            list.Add(user?.Identity.GetType());
            list.Add(user?.Identity.AuthenticationType);

            return list.ExpandAndToString("\r\n");
        }
    }
}