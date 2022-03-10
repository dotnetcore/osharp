// -----------------------------------------------------------------------
//  <copyright file="CustomLazyCaptcha.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2022 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-03-10 13:18</last-date>
// -----------------------------------------------------------------------

using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Storage;

using Microsoft.Extensions.Options;


namespace OSharp.Hosting.Utils
{
    public class CustomLazyCaptcha : ICaptcha
    {
        public CaptchaData Generate(string captchaId)
        {
            throw new System.NotImplementedException();
        }

        public bool Validate(string captchaId, string code)
        {
            throw new System.NotImplementedException();
        }
    }
}
