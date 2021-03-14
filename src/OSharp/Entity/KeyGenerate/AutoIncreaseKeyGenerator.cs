// -----------------------------------------------------------------------
//  <copyright file="IntKeyGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-14 13:37</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Entity.KeyGenerate
{
    /// <summary>
    /// 自增长int主键生成器
    /// </summary>
    public class AutoIncreaseKeyGenerator : IKeyGenerator<int>
    {
        /// <summary>
        /// 获取一个<see cref="int"/>类型的主键数据
        /// </summary>
        /// <returns></returns>
        public int Create()
        {
            return 0;
        }
    }
}