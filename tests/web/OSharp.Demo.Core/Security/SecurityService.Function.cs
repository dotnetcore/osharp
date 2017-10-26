// -----------------------------------------------------------------------
//  <copyright file="SecurityService.Function.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-10-24 23:53</last-date>
// -----------------------------------------------------------------------

using System.Linq;

using OSharp.Infrastructure;


namespace OSharp.Demo.Security
{
    partial class SecurityService
    {
        /// <summary>
        /// 获取 功能数据集
        /// </summary>
        public IQueryable<Function> Functions
        {
            get { return _functionRepository.Query(); }
        }
    }
}