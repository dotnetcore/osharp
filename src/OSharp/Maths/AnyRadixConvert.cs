// -----------------------------------------------------------------------
//  <copyright file="AnyRadixConvert.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 14:17</last-date>
// -----------------------------------------------------------------------

using System;
using System.Globalization;

using OSharp.Extensions;
using OSharp.Properties;


namespace OSharp.Maths
{
    /// <summary>
    /// 任意[2,62]进制转换器
    /// </summary>
    public static class AnyRadixConvert
    {
        private const string BaseChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 将指定基数的数字的字符串表示形式转换为等效的64位有符号整数
        /// </summary>
        /// <param name="value">指定基数的数字的字符串表示</param>
        /// <param name="fromRadix">字符串的基数，必须为[2,62]范围内</param>
        /// <returns>等效于value的数值的64位有符号整数</returns>
        public static ulong X2H(string value, int fromRadix)
        {
            value.CheckNotNullOrEmpty("value");
            fromRadix.CheckBetween("fromRadix", 2, 62, true, true);

            value = value.Trim();
            string baseChar = BaseChar.Substring(0, fromRadix);
            ulong result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                string str = value[i].ToString();
                if (!baseChar.Contains(str))
                {
                    throw new ArgumentException(string.Format(Resources.AnyRadixConvert_CharacterIsNotValid, str, fromRadix));
                }
                result += (ulong)baseChar.IndexOf(str, StringComparison.Ordinal) * (ulong)Math.Pow(baseChar.Length, value.Length - i - 1);
            }
            return result;
        }

        /// <summary>
        /// 将64位有符号整数形式的数值转换为指定基数的数值的字符串形式
        /// </summary>
        /// <param name="value">64位有符号整数形式的数值</param>
        /// <param name="toRadix">要转换的目标基数，必须为[2,62]范围内</param>
        /// <returns>指定基数的数值的字符串形式</returns>
        public static string H2X(ulong value, int toRadix)
        {
            toRadix.CheckBetween("fromRadix", 2, 62, true, true);
            if (value == 0)
            {
                return "0";
            }
            string baseChar = BaseChar.Substring(0, toRadix);
            string result = string.Empty;
            while (value > 0)
            {
                int index = (int)(value % (ulong)baseChar.Length);
                result = baseChar[index] + result;
                value = value / (ulong)baseChar.Length;
            }
            return result;
        }

        /// <summary>
        /// 任意进制转换，将源进制表示的value转换为目标进制，进制的字母顺序为先大写后小写
        /// </summary>
        /// <param name="value">要转换的数据</param>
        /// <param name="fromRadix">源进制数，必须为[2,62]范围内</param>
        /// <param name="toRadix">目标进制数，必须为[2,62]范围内</param>
        /// <returns></returns>
        public static string X2X(string value, int fromRadix, int toRadix)
        {
            value.CheckNotNullOrEmpty("value");
            fromRadix.CheckBetween("fromRadix", 2, 62, true, true);
            toRadix.CheckBetween("toRadix", 2, 62, true, true);

            ulong num = X2H(value, fromRadix);
            return H2X(num, toRadix);
        }

        /// <summary>
        /// 10进制数字转换为16进制字符串
        /// </summary>
        /// <param name="value">10进制数</param>
        /// <returns>16进制数的字符串</returns>
        public static string _10To16(int value)
        {
            value.CheckGreaterThan("value", 0, true);
            string str = X2X(value.ToString(CultureInfo.InvariantCulture), 10, 16);
            return str.IsNullOrEmpty() ? "0" : str[0] == '0' ? str : '0' + str;
        }

        /// <summary>
        /// 16进制字符串转换为10进制数字
        /// </summary>
        public static int _16To10(string value)
        {
            value = value.ToUpper();
            return X2X(value, 16, 10).CastTo<int>();
        }
    }
}