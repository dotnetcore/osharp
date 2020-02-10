// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthorization.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-11 1:05</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Authorization
{
    /// <summary>
    /// 功能权限验证类
    /// </summary>
    public class FunctionAuthorization : FunctionAuthorizationBase
    {
        /// <summary>
        /// 初始化一个<see cref="FunctionAuthorizationBase"/>类型的新实例
        /// </summary>
        public FunctionAuthorization(IFunctionAuthCache functionAuthCache)
            : base(functionAuthCache)
        { }
    }
}