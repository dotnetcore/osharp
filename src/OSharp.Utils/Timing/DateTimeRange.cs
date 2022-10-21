// -----------------------------------------------------------------------
//  <copyright file="DateTimeRange.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 14:16</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Timing
{
    /// <summary>
    /// 表示一个时间范围
    /// </summary>
    [Serializable]
    public class DateTimeRange
    {
        /// <summary>
        /// 初始化一个<see cref="DateTimeRange"/>类型的新实例
        /// </summary>
        public DateTimeRange()
            : this(DateTime.MinValue, DateTime.MaxValue)
        { }

        /// <summary>
        /// 初始化一个<see cref="DateTimeRange"/>类型的新实例
        /// </summary>
        public DateTimeRange(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// 获取或设置 起始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 获取或设置 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 获取 昨天的时间范围
        /// </summary>
        public static DateTimeRange Yesterday
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRange(now.Date.AddDays(-1), now.Date.AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 今天的时间范围
        /// </summary>
        public static DateTimeRange Today
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRange(now.Date.Date, now.Date.AddDays(1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 明天的时间范围
        /// </summary>
        public static DateTimeRange Tomorrow
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRange(now.Date.AddDays(1), now.Date.AddDays(2).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 上周的时间范围
        /// </summary>
        public static DateTimeRange LastWeek
        {
            get
            {
                DateTime now = DateTime.Now;
                DayOfWeek[] weeks =
                {
                    DayOfWeek.Sunday,
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday
                };
                int index = Array.IndexOf(weeks, now.DayOfWeek);
                return new DateTimeRange(now.Date.AddDays(-index - 7), now.Date.AddDays(-index).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 本周的时间范围
        /// </summary>
        public static DateTimeRange ThisWeek
        {
            get
            {
                DateTime now = DateTime.Now;
                DayOfWeek[] weeks =
                {
                    DayOfWeek.Sunday,
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday
                };
                int index = Array.IndexOf(weeks, now.DayOfWeek);
                return new DateTimeRange(now.Date.AddDays(-index), now.Date.AddDays(7 - index).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 下周的时间范围
        /// </summary>
        public static DateTimeRange NextWeek
        {
            get
            {
                DateTime now = DateTime.Now;
                DayOfWeek[] weeks =
                {
                    DayOfWeek.Sunday,
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday
                };
                int index = Array.IndexOf(weeks, now.DayOfWeek);
                return new DateTimeRange(now.Date.AddDays(-index + 7), now.Date.AddDays(14 - index).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 上个月的时间范围
        /// </summary>
        public static DateTimeRange LastMonth
        {
            get
            {
                DateTime now = DateTime.Now;
                DateTime startTime = now.Date.AddDays(-now.Day + 1).AddMonths(-1);
                DateTime endTime = startTime.AddMonths(1).AddMilliseconds(-1);
                return new DateTimeRange(startTime, endTime);
            }
        }

        /// <summary>
        /// 获取 本月的时间范围
        /// </summary>
        public static DateTimeRange ThisMonth
        {
            get
            {
                DateTime now = DateTime.Now;
                DateTime startTime = now.Date.AddDays(-now.Day + 1);
                DateTime endTime = startTime.AddMonths(1).AddMilliseconds(-1);
                return new DateTimeRange(startTime, endTime);
            }
        }

        /// <summary>
        /// 获取 下个月的时间范围
        /// </summary>
        public static DateTimeRange NextMonth
        {
            get
            {
                DateTime now = DateTime.Now;
                DateTime startTime = now.Date.AddDays(-now.Day + 1).AddMonths(1);
                DateTime endTime = startTime.AddMonths(1).AddMilliseconds(-1);
                return new DateTimeRange(startTime, endTime);
            }
        }

        /// <summary>
        /// 获取 上一年的时间范围
        /// </summary>
        public static DateTimeRange LastYear
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRange(new DateTime(now.Year - 1, 1, 1), new DateTime(now.Year, 1, 1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 本年的时间范围
        /// </summary>
        public static DateTimeRange ThisYear
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRange(new DateTime(now.Year, 1, 1), new DateTime(now.Year + 1, 1, 1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 下一年的时间范围
        /// </summary>
        public static DateTimeRange NextYear
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRange(new DateTime(now.Year + 1, 1, 1), new DateTime(now.Year + 2, 1, 1).AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 相对当前时间过去30天的时间范围
        /// </summary>
        public static DateTimeRange Last30Days
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRange(now.AddDays(-30), now);
            }
        }

        /// <summary>
        /// 获取 截止到昨天的最近30天的天数范围
        /// </summary>
        public static DateTimeRange Last30DaysExceptToday
        {
            get
            {
                var now = DateTime.Now;
                return new DateTimeRange(now.Date.AddDays(-30), now.Date.AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 获取 相对当前时间过去7天的时间范围
        /// </summary>
        public static DateTimeRange Last7Days
        {
            get
            {
                DateTime now = DateTime.Now;
                return new DateTimeRange(now.AddDays(-7), now);
            }
        }

        /// <summary>
        /// 获取 截止到昨天的最近7天的天数范围
        /// </summary>
        public static DateTimeRange Last7DaysExceptToday
        {
            get
            {
                var now = DateTime.Now;
                return new DateTimeRange(now.Date.AddDays(-7), now.Date.AddMilliseconds(-1));
            }
        }

        /// <summary>
        /// 返回表示当前 <see cref="T:System.Object"/> 的 <see cref="T:System.String"/>。
        /// </summary>
        /// <returns>
        /// <see cref="T:System.String"/>，表示当前的 <see cref="T:System.Object"/>。
        /// </returns>
        public override string ToString()
        {
            return string.Format("[{0} - {1}]", StartTime, EndTime);
        }
    }
}