// -----------------------------------------------------------------------
//  <copyright file="VerifyCodeHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-29 3:45</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Http;

using OSharp.AspNetCore.Http.Extensions;
using OSharp.Data;
using OSharp.Dependency;


namespace OSharp.AspNetCore.Infrastructure
{
    /// <summary>
    /// 验证码处理类
    /// </summary>
    public static class VerifyCodeHandler
    {
        /// <summary>
        /// 校验验证码有效性
        /// </summary>
        /// <param name="code">要校验的验证码</param>
        /// <param name="cleanIfFited">校验成功后是否清除</param>
        /// <returns></returns>
        public static bool CheckCode(string code, bool cleanIfFited = false)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            const string name = OsharpConstants.VerifyCodeSessionKey;
            ISession session = ServiceLocator.Instance.HttpContext().Session;
            string sessionCode = session.GetString(name);
            bool fited = sessionCode != null && sessionCode.Equals(code, StringComparison.OrdinalIgnoreCase);
            if (fited && cleanIfFited)
            {
                session.Remove(name);
            }
            return fited;
        }

        /// <summary>
        /// 设置验证码到Session中
        /// </summary>
        public static void SetCode(string code)
        {
            const string name = OsharpConstants.VerifyCodeSessionKey;
            ServiceLocator.Instance.HttpContext().Session.SetString(name, code);
        }
    }
}