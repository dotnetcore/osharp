// -----------------------------------------------------------------------
//  <copyright file="IdentityExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-16 11:53</last-date>
// -----------------------------------------------------------------------

using System.Linq;

using Microsoft.AspNetCore.Identity;

using OSharp.Collections;
using OSharp.Data;


namespace OSharp.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// 将<see cref="IdentityResult"/>转化为<see cref="OperationResult"/>
        /// </summary>
        public static OperationResult ToOperationResult(this IdentityResult identityResult)
        {
            return identityResult.Succeeded
                ? new OperationResult(OperationResultType.Success)
                : new OperationResult(OperationResultType.Error,
                    identityResult.Errors.Select(m => m.Description).ExpandAndToString());
        }

        /// <summary>
        /// 将<see cref="IdentityResult"/>转化为<see cref="OperationResult{TUser}"/>
        /// </summary>
        public static OperationResult<TUser> ToOperationResult<TUser>(this IdentityResult identityResult, TUser user)
        {
            return identityResult.Succeeded
                ? new OperationResult<TUser>(OperationResultType.Success, "Success", user)
                : new OperationResult<TUser>(OperationResultType.Error,
                    identityResult.Errors.Select(m => m.Description).ExpandAndToString());
        }

        /// <summary>
        /// 快速创建错误信息的IdentityResult
        /// </summary>
        public static IdentityResult Failed(this IdentityResult identityResult, params string[] errors)
        {
            var identityErrors = identityResult.Errors;
            identityErrors = identityErrors.Union(errors.Select(m => new IdentityError() { Description = m }));
            return IdentityResult.Failed(identityErrors.ToArray());
        }

        /// <summary>
        /// 获取IdentityResult的错误信息
        /// </summary>
        public static string ErrorMessage(this IdentityResult identityResult)
        {
            if (identityResult.Succeeded)
            {
                return null;
            }

            return identityResult.Errors.Select(m => m.Description).ExpandAndToString("; ");
        }
    }
}