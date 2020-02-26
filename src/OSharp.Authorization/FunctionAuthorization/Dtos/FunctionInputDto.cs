// -----------------------------------------------------------------------
//  <copyright file="FunctionInputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-15 17:25</last-date>
// -----------------------------------------------------------------------

using OSharp.Authorization.Functions;
using OSharp.Mapping;


namespace OSharp.Authorization.Dtos
{
    /// <summary>
    /// 输入DTO：功能信息
    /// </summary>
    [MapTo(typeof(Function))]
    public class FunctionInputDto : FunctionInputDtoBase
    { }
}