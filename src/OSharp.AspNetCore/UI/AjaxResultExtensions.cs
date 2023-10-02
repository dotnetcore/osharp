// -----------------------------------------------------------------------
//  <copyright file="AjaxResultExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-01 20:39</last-date>
// -----------------------------------------------------------------------

namespace OSharp.AspNetCore.UI;

/// <summary>
/// <see cref="AjaxResult"/>扩展方法
/// </summary>
public static class AjaxResultExtensions
{
    /// <summary>
    /// 将业务操作结果转ajax操作结果，并处理强类型的 OperationResult.Data
    /// </summary>
    public static AjaxResult<T> ToAjaxResult<T>(this OperationResult<T> result, Func<T, T> dataFunc = null)
    {
        string content = result.Message ?? result.ResultType.ToDescription();
        AjaxResultType type = result.ResultType.ToAjaxResultType();
        T data = dataFunc == null ? result.Data : dataFunc(result.Data);
        return new AjaxResult<T>(content, type, data);
    }

    /// <summary>
    /// 将业务操作结果转ajax操作结果，并处理强类型的 OperationResult.Data
    /// </summary>
    public static AjaxResult<TResult> ToAjaxResult<T, TResult>(this OperationResult<T> result, Func<T, TResult> dataFunc = null)
    {
        string content = result.Message ?? result.ResultType.ToDescription();
        AjaxResultType type = result.ResultType.ToAjaxResultType();
        TResult data;
        if (dataFunc == null)
        {
            if (result.Data is TResult data2)
            {
                data = data2;
            }
            else
            {
                data = default;
            }
        }
        else
        {
            data = dataFunc(result.Data);
        }

        return new AjaxResult<TResult>(content, type, data);
    }

    /// <summary>
    /// 将业务操作结果转ajax操作结果，可确定是否包含Data
    /// </summary>
    /// <param name="result">业务操作结果</param>
    /// <param name="containsData">是否包含Data，默认不包含</param>
    /// <returns></returns>
    public static AjaxResult ToAjaxResult(this OperationResult result, bool containsData = false)
    {
        string content = result.Message ?? result.ResultType.ToDescription();
        AjaxResultType type = result.ResultType.ToAjaxResultType();
        return containsData ? new AjaxResult(content, type, result.Data) : new AjaxResult(content, type);
    }

    /// <summary>
    /// 将业务操作结果转ajax操作结果，会将 object 类型的 OperationResult.Data 转换为强类型 T，再通过 dataFunc 进行进一步处理
    /// </summary>
    public static AjaxResult<T> ToAjaxResult<T>(this OperationResult result, Func<T, T> dataFunc)
    {
        string content = result.Message ?? result.ResultType.ToDescription();
        AjaxResultType type = result.ResultType.ToAjaxResultType();
        T data = default;
        if (result.Data != null)
        {
            if (dataFunc != null && result.Data is T resultData)
            {
                data = dataFunc(resultData);
            }
        }
        return new AjaxResult<T>(content, type, data);
    }

    /// <summary>
    /// 把业务结果类型<see cref="OperationResultType"/>转换为Ajax结果类型<see cref="AjaxResultType"/>
    /// </summary>
    public static AjaxResultType ToAjaxResultType(this OperationResultType resultType)
    {
        switch (resultType)
        {
            case OperationResultType.Success:
                return AjaxResultType.Success;
            case OperationResultType.NoChanged:
                return AjaxResultType.Info;
            default:
                return AjaxResultType.Error;
        }
    }

    /// <summary>
    /// 判断业务结果类型是否是Error结果
    /// </summary>
    public static bool IsError(this OperationResultType resultType)
    {
        return resultType == OperationResultType.QueryNull
            || resultType == OperationResultType.ValidError
            || resultType == OperationResultType.Error;
    }
}
