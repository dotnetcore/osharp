// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>王明杰</last-editor>
//  <last-date>2014:07:09 11:40</last-date>
// -----------------------------------------------------------------------

using System;
using System.Text;


namespace OSharp.Extensions
{
    /// <summary>
    /// StringBuilder 扩展方法类
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// 去除<seealso cref="StringBuilder"/>开头的空格
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <returns>返回修改后的StringBuilder，主要用于链式操作</returns>
        public static StringBuilder TrimStart(this StringBuilder sb)
        {
            return sb.TrimStart(' ');
        }

        /// <summary>
        /// 去除<seealso cref="StringBuilder"/>开头的指定<seealso cref="char"/>
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="c">要去掉的<seealso cref="char"/></param>
        /// <returns></returns>
        public static StringBuilder TrimStart(this StringBuilder sb, char c)
        {
            sb.CheckNotNull("sb");
            if (sb.Length == 0)
                return sb;
            while (c.Equals(sb[0]))
            {
                sb.Remove(0, 1);
            }
            return sb;
        }

        /// <summary>
        /// 去除<seealso cref="StringBuilder"/>开头的指定字符数组
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="cs">要去掉的字符数组</param>
        /// <returns></returns>
        public static StringBuilder TrimStart(this StringBuilder sb, char[] cs)
        {
            cs.CheckNotNull("chars");
            return sb.TrimStart(new string(cs));
        }
        /// <summary>
        /// 去除<see cref="StringBuilder"/>开头的指定的<seealso cref="string"/>
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="str">要去掉的<seealso cref="string"/></param>
        /// <returns></returns>
        public static StringBuilder TrimStart(this StringBuilder sb, string str)
        {
            sb.CheckNotNull("sb");
            if (string.IsNullOrEmpty(str)
                || sb.Length == 0
                || str.Length > sb.Length)
                return sb;
            while (sb.SubString(0, str.Length).Equals(str))
            {
                sb.Remove(0, str.Length);
                if (str.Length>sb.Length)
                {
                    break;
                }
            }
            return sb;
        }

        /// <summary>
        /// 去除StringBuilder结尾的空格
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <returns>返回修改后的StringBuilder，主要用于链式操作</returns>
        public static StringBuilder TrimEnd(this StringBuilder sb)
        {
            return sb.TrimEnd(' ');
        }

        /// <summary>
        /// 去除<see cref="StringBuilder"/>结尾指定字符
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="c">要去掉的字符</param>
        /// <returns></returns>
        public static StringBuilder TrimEnd(this StringBuilder sb, char c)
        {
            sb.CheckNotNull("sb");
             if (sb.Length == 0)
                return sb;
            while (c.Equals(sb[sb.Length - 1]))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb;
        }

        /// <summary>
        /// 去除<see cref="StringBuilder"/>结尾指定字符数组
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="chars">要去除的字符数组</param>
        /// <returns></returns>
        public static StringBuilder TrimEnd(this StringBuilder sb, char[] chars)
        {
            chars.CheckNotNull("chars");
            return sb.TrimEnd(new string(chars));
        }

        /// <summary>
        /// 去除<see cref="StringBuilder"/>结尾指定字符串
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="str">要去除的字符串</param>
        /// <returns></returns>
        public static StringBuilder TrimEnd(this StringBuilder sb, string str)
        {
            sb.CheckNotNull("sb");
            if (string.IsNullOrEmpty(str)
                || sb.Length == 0
                || str.Length > sb.Length)
                return sb;
            while (sb.SubString(sb.Length-str.Length, str.Length).Equals(str))
            {
                sb.Remove(sb.Length-str.Length, str.Length);
                if (sb.Length<str.Length)
                {
                    break;
                }
            }
            return sb;
        }

        /// <summary>
        /// 去除StringBuilder两端的空格
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <returns>返回修改后的StringBuilder，主要用于链式操作</returns>
        public static StringBuilder Trim(this StringBuilder sb)
        {
            sb.CheckNotNull("sb");
            if (sb.Length == 0)
                return sb;
            return sb.TrimEnd().TrimStart();
        }

        /// <summary>
        /// 返回<see cref="StringBuilder"/>从起始位置指定长度的字符串
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="start">起始位置</param>
        /// <param name="length">长度</param>
        /// <returns>字符串</returns>
        /// <exception cref="OverflowException">超出字符串索引长度异常</exception>
        public static string SubString(this StringBuilder sb, int start, int length)
        {
            sb.CheckNotNull("sb");
            if (start + length > sb.Length)
                throw new IndexOutOfRangeException("超出字符串索引长度");
            char[] cs = new char[length];
            for (int i = 0; i < length; i++)
            {
                cs[i] = sb[start + i];
            }
            return new string(cs);
        }
    }
}
