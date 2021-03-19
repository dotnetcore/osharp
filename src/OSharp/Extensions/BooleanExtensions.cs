// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:04 8:01</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Extensions
{
    /// <summary>
    /// 布尔值<see cref="Boolean"/>类型的扩展辅助操作类
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// 把布尔值转换为小写字符串
        /// </summary>
        public static string ToLower(this bool value)
        {
            return value.ToString().ToLower();
        }

        /// <summary>
        /// 如果条件成立，则抛出异常
        /// </summary>
        public static void TrueThrow(this bool flag, Exception exception)
        {
            if (flag)
            {
                throw exception;
            }
        }
    }
}