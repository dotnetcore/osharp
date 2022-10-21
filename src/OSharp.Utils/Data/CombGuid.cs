// -----------------------------------------------------------------------
//  <copyright file="CombHelper.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-07-18 16:17</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Data
{
    /// <summary>
    /// COMB（<see cref="Guid"/>与<see cref="DateTime"/>混合构成的可排序<see cref="Guid"/>）类型操作类
    /// </summary>
    public static class CombGuid
    {
        /// <summary>
        /// 返回Guid用于数据库操作，特定的时间代码可以提高检索效率
        /// </summary>
        /// <returns>COMB类型 Guid 数据</returns>
        public static Guid NewGuid()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            DateTime dtBase = new DateTime(1900, 1, 1);
            DateTime dtNow = DateTime.Now;
            //获取用于生成byte字符串的天数与毫秒数
            TimeSpan days = new TimeSpan(dtNow.Ticks - dtBase.Ticks);
            TimeSpan msecs = new TimeSpan(dtNow.Ticks - new DateTime(dtNow.Year, dtNow.Month, dtNow.Day).Ticks);
            //转换成byte数组
            //注意SqlServer的时间计数只能精确到1/300秒
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            //反转字节以符合SqlServer的排序
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            //把字节复制到Guid中
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
            return new Guid(guidArray);
        }

        /// <summary>
        /// 从SQL Server 返回的Guid中生成时间信息
        /// </summary>
        public static DateTime GetDateFrom(Guid id)
        {
            DateTime baseDate = new DateTime(1900, 1, 1);
            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = id.ToByteArray();

            // 将GUID的日期部分复制到相应的字节数组。 
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            // 将数组反转，以将它们排列成适当的顺序。 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // 将字节转换为int 
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);

            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }
    }
}