// -----------------------------------------------------------------------
//  <copyright file="StringTrimModelBinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-01 17:20</last-date>
// -----------------------------------------------------------------------

using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using OSharp.Data;


namespace OSharp.AspNetCore.Mvc.ModelBinding
{
    /// <summary>
    /// 提供对字符串前后空白进行Trim操作的模型绑定能力
    /// </summary>
    public class StringTrimModelBinder : IModelBinder
    {
        /// <summary>Attempts to bind a model.</summary>
        /// <param name="bindingContext">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext" />.</param>
        /// <returns>
        /// <para>
        /// A <see cref="T:System.Threading.Tasks.Task" /> which will complete when the model binding process completes.
        /// </para>
        /// <para>
        /// If model binding was successful, the <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext.Result" /> should have
        /// <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult.IsModelSet" /> set to <c>true</c>.
        /// </para>
        /// <para>
        /// A model binder that completes successfully should set <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingContext.Result" /> to
        /// a value returned from <see cref="M:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBindingResult.Success(System.Object)" />.
        /// </para>
        /// </returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Check.NotNull(bindingContext, nameof(bindingContext));

            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            Type underlyingOrModelType = bindingContext.ModelMetadata.UnderlyingOrModelType;
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            try
            {
                string firstValue = valueProviderResult.FirstValue;
                object model;
                if (string.IsNullOrWhiteSpace(firstValue))
                {
                    model = null;
                }
                else
                {
                    if (underlyingOrModelType != typeof(string))
                    {
                        throw new MulticastNotSupportedException();
                    }
                    model = firstValue.Trim();
                }
                bindingContext.Result = ModelBindingResult.Success(model);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                if (!(exception is FormatException) && exception.InnerException != null)
                    exception = ExceptionDispatchInfo.Capture(exception.InnerException).SourceException;
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, exception, bindingContext.ModelMetadata);
                return Task.CompletedTask;
            }
        }
    }
}