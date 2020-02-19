// -----------------------------------------------------------------------
//  <copyright file="StringToCollectionConverter.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-19 19:31</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using OSharp.Collections;


namespace OSharp.AutoMapper.Converters
{
    /// <summary>
    /// 转换器：<see cref="String"/> to <see cref="ICollection{String}"/>
    /// </summary>
    public class StringToCollectionConverter : IValueConverter<ICollection<string>, string>, IValueConverter<string, ICollection<string>>
    {
        /// <summary>
        /// Perform conversion from source member value to destination member value
        /// </summary>
        /// <param name="sourceMember">Source member object</param>
        /// <param name="context">Resolution context</param>
        /// <returns>Destination member value</returns>
        public string Convert(ICollection<string> sourceMember, ResolutionContext context)
        {
            if (sourceMember == null || !sourceMember.Any())
            {
                return null;
            }

            return sourceMember.ExpandAndToString();
        }

        #region Implementation of IValueConverter<in string,out ICollection<string>>

        /// <summary>
        /// Perform conversion from source member value to destination member value
        /// </summary>
        /// <param name="sourceMember">Source member object</param>
        /// <param name="context">Resolution context</param>
        /// <returns>Destination member value</returns>
        public ICollection<string> Convert(string sourceMember, ResolutionContext context)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrWhiteSpace(sourceMember))
            {
                return list;
            }

            sourceMember = sourceMember.Trim();
            list.AddRange(sourceMember.Split(',', StringSplitOptions.RemoveEmptyEntries).Distinct());

            return list;
        }

        #endregion
    }
}