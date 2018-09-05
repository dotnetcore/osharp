/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/ArcherTrister/LeXun.Security.OAuth
 * for more information concerning the license and the contributors participating to this project.
 */

namespace OSharp.Identity.OAuth2.Baidu
{
    /// <summary>
    /// Contains constants specific to the <see cref="BaiduAuthenticationHandler"/>.
    /// </summary>
    public static class BaiduAuthenticationConstants
    {
        public static class Claims
        {
            /// <summary>
            /// 当前登录用户的数字ID
            /// </summary>
            public const string UserId = "urn:baidu:userid";

            /// <summary>
            /// 当前登录用户的用户名，值可能为空。
            /// </summary>
            public const string UserName = "urn:baidu:username";

            /// <summary>
            /// 用户真实姓名，可能为空。
            /// </summary>
            public const string RealName = "urn:baidu:realname";

            /// <summary>
            /// 当前登录用户的头像
            /// </summary>
            public const string Portrait = "urn:baidu:portrait";

            /// <summary>
            /// 自我简介，可能为空。
            /// </summary>
            public const string UserDetail = "urn:baidu:userdetail";

            /// <summary>
            /// 生日，以yyyy-mm-dd格式显示。
            /// </summary>
            public const string Birthday = "urn:baidu:birthday";

            /// <summary>
            /// 婚姻状况
            /// </summary>
            public const string Marriage = "urn:baidu:marriage";

            /// <summary>
            /// 血型
            /// </summary>
            public const string Blood = "urn:baidu:blood";

            /// <summary>
            /// 体型
            /// </summary>
            public const string Figure = "urn:baidu:figure";

            /// <summary>
            /// 星座
            /// </summary>
            public const string Constellation = "urn:baidu:constellation";

            /// <summary>
            /// 学历
            /// </summary>
            public const string Education = "urn:baidu:education";

            /// <summary>
            /// 当前职业
            /// </summary>
            public const string Trade = "urn:baidu:trade";

            /// <summary>
            /// 职位
            /// </summary>
            public const string Job = "urn:baidu:job";
        }
    }
}