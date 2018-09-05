/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/ArcherTrister/LeXun.Security.OAuth
 * for more information concerning the license and the contributors participating to this project.
 */

namespace OSharp.Identity.OAuth2.Alipay
{
    /// <summary>
    /// Contains constants specific to the <see cref="AlipayAuthenticationHandler"/>.
    /// </summary>
    public static class AlipayAuthenticationConstants
    {
        public static class Claims
        {
            /// <summary>
            /// 当前登录用户的数字ID
            /// </summary>
            public const string UserId = "urn:alipay:user_id";

            /// <summary>
            /// 用户昵称
            /// </summary>
            public const string NickName = "urn:alipay:nick_name";

            /// <summary>
            /// 用户头像地址
            /// </summary>
            public const string Avatar = "urn:alipay:avatar";

            /// <summary>
            /// 省份名称
            /// </summary>
            public const string Province = "urn:alipay:province";

            /// <summary>
            /// 市名称
            /// </summary>
            public const string City = "urn:alipay:city";

            /// <summary>
            /// 是否是学生(选填)
            /// </summary>
            public const string IsStudentCertified = "urn:alipay:is_student_certified";

            /// <summary>
            /// 用户类型（1/2） 1代表公司账户2代表个人账户
            /// </summary>
            public const string UserType = "urn:alipay:user_type";

            /// <summary>
            /// 用户状态（Q/T/B/W）。
            /// Q代表快速注册用户
            /// T代表已认证用户
            /// B代表被冻结账户
            /// W代表已注册，未激活的账户
            /// </summary>
            public const string UserStatus = "urn:alipay:user_status";

            /// <summary>
            /// 是否通过实名认证。T是通过 F是没有实名认证。
            /// </summary>
            public const string IsCertified = "urn:alipay:is_certified";

            ///// <summary>
            ///// 【注意】只有is_certified为T的时候才有意义，否则不保证准确性. 性别（F：女性；M：男性）。
            ///// </summary>
            //public const string Gender = "urn:alipay:gender";
        }
    }
}