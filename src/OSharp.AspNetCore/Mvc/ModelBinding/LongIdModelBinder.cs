// -----------------------------------------------------------------------
//  <copyright file="SnowflakeIdModelBinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2023 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2023-10-05 0:19</last-date>
// -----------------------------------------------------------------------

using System.Globalization;


namespace OSharp.AspNetCore.Mvc.ModelBinding
{
    public class LongIdModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // 获取传入的ID值作为字符串
            string valueAsString = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            if (string.IsNullOrEmpty(valueAsString))
            {
                return Task.CompletedTask;
            }

            if (long.TryParse(valueAsString, NumberStyles.Integer, CultureInfo.InvariantCulture, out long id))
            {
                // 成功解析为long，将其设置为模型的值
                bindingContext.Result = ModelBindingResult.Success(id);
            }
            else
            {
                // 解析失败，返回错误
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid Snowflake ID format.");
            }
            return Task.CompletedTask;
        }
    }
}
