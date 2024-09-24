// -----------------------------------------------------------------------
//  <copyright file="LocalizedDescriptionAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2024 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>gmf</last-editor>
//  <last-date>2024-09-06 10:09</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;


namespace OSharp.Localized
{
    /// <summary>
    /// 本地化描述类特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;
        private readonly ResourceManager _resourceManager;

        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resourceKey = resourceKey;
            _resourceManager = new ResourceManager(resourceType);
        }

        /// <summary>Gets the description stored in this attribute.</summary>
        /// <returns>The description stored in this attribute.</returns>
        public override string Description
        {
            get
            {
                var description = _resourceManager.GetString(_resourceKey, CultureInfo.CurrentCulture);
                return string.IsNullOrEmpty(description) ? $"[{_resourceKey}]" : description;
            }
        }
    }
}
