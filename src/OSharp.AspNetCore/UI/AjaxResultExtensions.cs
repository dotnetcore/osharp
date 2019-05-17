// -----------------------------------------------------------------------
//  <copyright file="AjaxResultExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-01 20:39</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Data;
using OSharp.Extensions;


namespace OSharp.AspNetCore.UI
{
    /// <summary>
    /// <see cref="AjaxResult"/>扩展方法
    /// </summary>
    public static class AjaxResultExtensions
    {
        /// <summary>
        /// 将业务操作结果转ajax操作结果，并处理强类型的 <see cref="OperationResult.Data"/>
        /// </summary>
        public static AjaxResult ToAjaxResult<T>(this OperationResult<T> result, Func<T, object> dataFunc = null)
        {
            string content = result.Message ?? result.ResultType.ToDescription();
            AjaxResultType type = result.ResultType.ToAjaxResultType();
            object data = dataFunc == null ? result.Data : dataFunc(result.Data);
            return new AjaxResult(content, type, data);
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
        /// 将业务操作结果转ajax操作结果，会将 object 类型的 <see cref="OperationResult.Data"/> 转换为强类型 T，再通过 dataFunc 进行进一步处理
        /// </summary>
        public static AjaxResult ToAjaxResult<T>(this OperationResult result, Func<T, object> dataFunc)
        {
            string content = result.Message ?? result.ResultType.ToDescription();
            AjaxResultType type = result.ResultType.ToAjaxResultType();
            object data = null;
            if (result.Data != null)
            {
                if (dataFunc != null && result.Data is T resultData)
                {
                    data = dataFunc(resultData);
                }
            }
            return new AjaxResult(content, type, data);
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
}