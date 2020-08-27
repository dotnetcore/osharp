// -----------------------------------------------------------------------
//  <copyright file="DateTimeExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 14:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using OSharp.Data;


namespace OSharp.Timing
{
    /// <summary>
    /// 时间扩展操作类
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 当前时间是否周末
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime dateTime)
        {
            DayOfWeek[] weeks = { DayOfWeek.Saturday, DayOfWeek.Sunday };
            return weeks.Contains(dateTime.DayOfWeek);
        }

        /// <summary>
        /// 当前时间是否工作日
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns></returns>
        public static bool IsWeekday(this DateTime dateTime)
        {
            DayOfWeek[] weeks = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
            return weeks.Contains(dateTime.DayOfWeek);
        }

        /// <summary>
        /// 获取时间相对唯一字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="milsec">是否使用毫秒</param>
        /// <returns></returns>
        public static string ToUniqueString(this DateTime dateTime, bool milsec = false)
        {
            int seconds = dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second;
            string value = $"{dateTime:yy}{dateTime.DayOfYear}{seconds}";
            return milsec ? value + dateTime.ToString("fff") : value;
        }

        /// <summary>
        /// 将时间转换为JS时间格式(Date.getTime())
        /// </summary>
        public static string ToJsGetTime(this DateTime dateTime, bool milsec = true)
        {
            DateTime utc = dateTime.ToUniversalTime();
            TimeSpan span = utc.Subtract(new DateTime(1970, 1, 1));
            return Math.Round(milsec ? span.TotalMilliseconds : span.TotalSeconds).ToString();
        }

        /// <summary>
        /// 将JS时间格式的数值转换为时间
        /// </summary>
        public static DateTime FromJsGetTime(this long jsTime)
        {
            int length = jsTime.ToString().Length;
            Check.Required<ArgumentException>(length == 10 || length == 13, "JS时间数值的长度不正确，必须为10位或13位");
            DateTime start = new DateTime(1970, 1, 1);
            DateTime result = length == 10 ? start.AddSeconds(jsTime) : start.AddMilliseconds(jsTime);
            return result.ToUniversalTime();
        }
    }
}