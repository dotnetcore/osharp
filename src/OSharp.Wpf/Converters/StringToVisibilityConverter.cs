// -----------------------------------------------------------------------
//  <copyright file="StringToVisibilityConverter.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-02-05 1:33</last-date>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace OSharp.Wpf.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        /// <summary>转换值。</summary>
        /// <param name="value">绑定源生成的值。</param>
        /// <param name="targetType">绑定目标属性的类型。</param>
        /// <param name="parameter">要使用的转换器参数。</param>
        /// <param name="culture">要用在转换器中的区域性。</param>
        /// <returns>转换后的值。 如果该方法返回 <see langword="null" />，则使用有效的 null 值。</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }
            string bindingValue = value.ToString();
            string parameterValue = parameter.ToString();
            bool flag = bindingValue != null && bindingValue.Equals(parameterValue, StringComparison.InvariantCultureIgnoreCase);
            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>转换值。</summary>
        /// <param name="value">绑定目标生成的值。</param>
        /// <param name="targetType">要转换为的类型。</param>
        /// <param name="parameter">要使用的转换器参数。</param>
        /// <param name="culture">要用在转换器中的区域性。</param>
        /// <returns>转换后的值。 如果该方法返回 <see langword="null" />，则使用有效的 null 值。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return null;
            }

            Visibility visibility = (Visibility)value;
            return visibility == Visibility.Visible ? parameter.ToString() : null;
        }
    }
}