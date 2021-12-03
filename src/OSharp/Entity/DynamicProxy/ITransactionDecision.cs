// -----------------------------------------------------------------------
//  <copyright file="ITransactionDecision.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-09-07 20:37</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Dependency;


namespace OSharp.Entity.DynamicProxy
{
    /// <summary>定义操作事务提交决策者，根据操作结果类型及值决定是否提交事务</summary>
    [MultipleDependency]
    public interface ITransactionDecision : ISingletonDependency
    {
        /// <summary>确定指定的返回类型是否合适。</summary>
        /// <param name="returnType">返回类型</param>
        /// <returns>
        ///   <c>true</c> 如果指定的返回类型适合；否则，<c>false</c>.</returns>
        bool IsFit(Type returnType);

        /// <summary>根据返回结果决定当前事务是否可提交</summary>
        /// <param name="returnResult">操作返回结果</param>
        /// <returns>
        ///   <c>true</c> 如果事务可提交；否则，<c>false</c>.
        /// </returns>
        bool CanCommit(object returnResult);
    }
}