// -----------------------------------------------------------------------
//  <copyright file="InputDtoValidateExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-04-30 14:31</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using OSharp.Data;
using OSharp.Extensions;


namespace OSharp.Entity
{
    /// <summary>
    /// <see cref="IInputDto{TKey}"/>验证扩展 
    /// </summary>
    public static class InputDtoValidateExtensions
    {
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<PropertyInfo, ValidationAttribute[]>> _dict
            = new ConcurrentDictionary<Type, ConcurrentDictionary<PropertyInfo, ValidationAttribute[]>>();

        /// <summary>
        /// InputDto属性验证
        /// </summary>
        public static void Validate<TKey>(this IEnumerable<IInputDto<TKey>> dtos)
        {
            IInputDto<TKey>[] inputDtos = dtos as IInputDto<TKey>[] ?? dtos.ToArray();
            Check.NotNull(inputDtos, nameof(dtos));
            foreach (IInputDto<TKey> dto in inputDtos)
            {
                dto.Validate();
            }
        }
    }
}