// -----------------------------------------------------------------------
//  <copyright file="HiddenApiFilter.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-31 13:30</last-date>
// -----------------------------------------------------------------------

using System;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using OSharp.Reflection;

using Swashbuckle.AspNetCore.SwaggerGen;


namespace OSharp.Swagger
{
    /// <summary>
    /// Swagger隐藏过滤器
    /// </summary>
    public class HiddenApiFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {
                if (apiDescription.TryGetMethodInfo(out MethodInfo method))
                {
                    if (method.ReflectedType.HasAttribute<HiddenApiAttribute>() || method.HasAttribute<HiddenApiAttribute>())
                    {
                        string key = $"/{apiDescription.RelativePath}";
                        if (key.Contains("?"))
                        {
                            int index = key.IndexOf("?", StringComparison.Ordinal);
                            key = key.Substring(0, index);
                        }

                        swaggerDoc.Paths.Remove(key);
                    }
                }
            }
        }
    }
}