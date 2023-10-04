// -----------------------------------------------------------------------
//  <copyright file="LongIdModelBinderProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2023 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2023-10-05 1:26</last-date>
// -----------------------------------------------------------------------

namespace OSharp.AspNetCore.Mvc.ModelBinding
{
    public class LongIdModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" /> based on <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext" />.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext" />.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder" />.</returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(long))
            {
                return new LongIdModelBinder();
            }

            return null;
        }
    }
}
