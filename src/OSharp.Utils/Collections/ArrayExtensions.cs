// -----------------------------------------------------------------------
//  <copyright file="ArrayExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-04 0:46</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Collections
{
    /// <summary>
    /// 数组扩展方法
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 复制一份二维数组的副本
        /// </summary>
        public static byte[,] Copy(this byte[,] bytes)
        {
            int width = bytes.GetLength(0), height = bytes.GetLength(1);
            byte[,] newBytes = new byte[width, height];
            Array.Copy(bytes, newBytes, bytes.Length);
            return newBytes;
        }
    }
}