// -----------------------------------------------------------------------
//  <copyright file="Check.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-16 23:06</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Data;

/// <summary>
/// 参数合法性检查类
/// </summary>
[DebuggerStepThrough]
public static class Check2
{
    /// <summary>
    /// 检查<see cref="IInputDto{TKey}"/>各属性的合法性，否则抛出<see cref="ValidationException"/>异常
    /// </summary>
    public static void Validate<TKey>(IInputDto<TKey> dto, string paramName)
    {
        Check.NotNull(dto, paramName);
        dto.Validate();
    }

    /// <summary>
    /// 检查<see cref="IInputDto{TKey}"/>各属性的合法性，否则抛出<see cref="ValidationException"/>异常
    /// </summary>
    public static void Validate<TInputDto, TKey>(TInputDto[] dtos, string paramName) where TInputDto : IInputDto<TKey>
    {
        Check.NotNull(dtos, paramName);
        foreach (TInputDto dto in dtos)
        {
            dto.Validate();
        }
    }
}