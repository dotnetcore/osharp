// -----------------------------------------------------------------------
//  <copyright file="OperationResultTransactionDecision.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-09-07 21:58</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using OSharp.Data;
using OSharp.Reflection;


namespace OSharp.Entity.DynamicProxy
{
    /// <summary>
    /// <see cref="OperationResult"/>操作结果事务决策者
    /// </summary>
    internal sealed class OperationResultTransactionDecision : ITransactionDecision
    {
        /// <summary>确定指定的返回类型是否合适。</summary>
        /// <param name="returnType">返回类型</param>
        /// <returns>
        ///   <c>true</c> 如果指定的返回类型适合；否则，<c>false</c>.</returns>
        public bool IsFit(Type returnType)
        {
            return returnType == typeof(OperationResult)
                || returnType == typeof(Task<OperationResult>)
                || returnType.IsBaseOn(typeof(OperationResult<>))
                || returnType.IsBaseOn(typeof(Task<>)) && returnType.GetGenericArguments()[0].IsBaseOn(typeof(OperationResult<>));
        }

        /// <summary>根据返回结果决定当前事务是否可提交</summary>
        /// <param name="returnResult">操作返回结果</param>
        /// <returns>
        ///   <c>true</c> 如果事务可提交；否则，<c>false</c>.
        /// </returns>
        public bool CanCommit(object returnResult)
        {
            if (returnResult is OperationResult result1)
            {
                return result1.Succeeded;
            }

            if (returnResult is Task<OperationResult> result2)
            {
                return result2.Result.Succeeded;
            }

            Type returnType = returnResult.GetType();
            if (returnType.IsBaseOn(typeof(OperationResult<>)))
            {
                dynamic result3 = returnResult;
                return (bool)result3.Succeeded;
            }

            if (returnType.IsBaseOn(typeof(Task<>)) && returnType.GetGenericArguments()[0].IsBaseOn(typeof(OperationResult<>)))
            {
                dynamic result4 = ((dynamic)returnResult).Result;
                return (bool)result4.Succeeded;
            }


            throw new NotImplementedException();
        }
    }
}