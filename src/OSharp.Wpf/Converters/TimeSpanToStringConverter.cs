// -----------------------------------------------------------------------
//  <copyright file="TimeSpanToStringConverter.cs" company="柳柳软件">
//      Copyright (c) 2015 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-12-30 21:17</last-date>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows.Data;


namespace OSharp.Wpf.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        /// <summary>
        /// 转换值。
        /// </summary>
        /// <returns>
        /// 转换后的值。如果该方法返回 null，则使用有效的 null 值。
        /// </returns>
        /// <param name="value">绑定源生成的值。</param><param name="targetType">绑定目标属性的类型。</param><param name="parameter">要使用的转换器参数。</param><param name="culture">要用在转换器中的区域性。</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan ts)
            {
                return ts.ToString(@"hh\:mm\:ss");
                //return $"{(int)ts.TotalHours}时{ts.Minutes.ToString("D2")}分{ts.Seconds.ToString("D2")}秒";
            }
            return "未开始";
        }

        /// <summary>
        /// 转换值。
        /// </summary>
        /// <returns>
        /// 转换后的值。如果该方法返回 null，则使用有效的 null 值。
        /// </returns>
        /// <param name="value">绑定目标生成的值。</param><param name="targetType">要转换到的类型。</param><param name="parameter">要使用的转换器参数。</param><param name="culture">要用在转换器中的区域性。</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}