// -----------------------------------------------------------------------
//  <copyright file="IVerifyCodeService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-19 17:15</last-date>
// -----------------------------------------------------------------------

using System.Drawing;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// 定义验证码处理功能
    /// </summary>
    public interface IVerifyCodeService
    {
        /// <summary>
        /// 校验验证码有效性
        /// </summary>
        /// <param name="code">要校验的验证码</param>
        /// <param name="id">验证码编号</param>
        /// <param name="removeIfSuccess">验证成功时是否移除</param>
        /// <returns></returns>
        bool CheckCode(string code, string id, bool removeIfSuccess = true);

        /// <summary>
        /// 设置验证码到Session中
        /// </summary>
        string SetCode(string code, int seconds = 60 * 3);

        /// <summary>
        /// 将图片序列化成字符串
        /// </summary>
        string GetImageString(Image image, string id);
    }
}