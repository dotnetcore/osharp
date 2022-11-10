// -----------------------------------------------------------------------
//  <copyright file="BooleanToNoVisibilityConverter.cs" company="柳柳软件">
//      Copyright (c) 2021 66SOFT. All rights reserved.
//  </copyright>
//  <site>https://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-11-02 21:00</last-date>
// -----------------------------------------------------------------------

namespace Gmf.WG.Client.Fsb.Converters;

public class BooleanToNoVisibilityConverter : IValueConverter
{
    /// <summary>转换值。</summary>
    /// <param name="value">绑定源生成的值。</param>
    /// <param name="targetType">绑定目标属性的类型。</param>
    /// <param name="parameter">要使用的转换器参数。</param>
    /// <param name="culture">要用在转换器中的区域性。</param>
    /// <returns>
    ///   转换后的值。
    ///    如果该方法返回 <see langword="null" />，则使用有效的 null 值。
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool flag)
        {
            return flag ? Visibility.Collapsed : Visibility.Visible;
        }

        return Visibility.Visible;
    }

    /// <summary>转换值。</summary>
    /// <param name="value">绑定目标生成的值。</param>
    /// <param name="targetType">要转换为的类型。</param>
    /// <param name="parameter">要使用的转换器参数。</param>
    /// <param name="culture">要用在转换器中的区域性。</param>
    /// <returns>
    ///   转换后的值。
    ///    如果该方法返回 <see langword="null" />，则使用有效的 null 值。
    /// </returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
        {
            return visibility == Visibility.Collapsed;
        }

        return false;
    }
}
