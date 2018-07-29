// -----------------------------------------------------------------------
//  <copyright file="VerifyCodeHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-29 3:45</last-date>
// -----------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Microsoft.Extensions.Caching.Distributed;

using OSharp.Data;
using OSharp.Dependency;
using OSharp.Extensions;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// 验证码处理类
    /// </summary>
    public static class VerifyCodeHandler
    {
        private const string Separator = "#$#";

        /// <summary>
        /// 校验验证码有效性
        /// </summary>
        /// <param name="code">要校验的验证码</param>
        /// <param name="id">验证码编号</param>
        /// <param name="removeIfSuccess">验证成功时是否移除</param>
        /// <returns></returns>
        public static bool CheckCode(string code, string id, bool removeIfSuccess = true)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            string key = $"{OsharpConstants.VerifyCodeKeyPrefix}_{id}";
            IDistributedCache cache = ServiceLocator.Instance.GetService<IDistributedCache>();
            bool flag = code.Equals(cache.GetString(key), StringComparison.OrdinalIgnoreCase);
            if (removeIfSuccess && flag)
            {
                cache.Remove(key);
            }
            return flag;
        }

        /// <summary>
        /// 设置验证码到Session中
        /// </summary>
        public static void SetCode(string code, out string id)
        {
            id = Guid.NewGuid().ToString("N");
            string key = $"{OsharpConstants.VerifyCodeKeyPrefix}_{id}";
            IDistributedCache cache = ServiceLocator.Instance.GetService<IDistributedCache>();
            const int seconds = 60 * 3;
            cache.SetString(key, code, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds) });
        }

        /// <summary>
        /// 将图片序列化成字符串
        /// </summary>
        public static string GetImageString(Image image, string id)
        {
            Check.NotNull(image, nameof(image));
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                string str = $"data:image/png;base64,{bytes.ToBase64String()}{Separator}{id}";
                return str.ToBase64String();
            }
        }
    }
}