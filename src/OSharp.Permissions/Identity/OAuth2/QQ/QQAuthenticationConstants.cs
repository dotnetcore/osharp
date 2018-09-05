/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/ArcherTrister/LeXun.Security.OAuth
 * for more information concerning the license and the contributors participating to this project.
 */

namespace OSharp.Identity.OAuth2.QQ
{
    /// <summary>
    /// Contains constants specific to the <see cref="QQAuthenticationHandler"/>.
    /// </summary>
    public static class QQAuthenticationConstants
    {
        public static class Claims
        {
            /// <summary>
            /// 用户在QQ空间的昵称。
            /// </summary>
            public const string NickName = "urn:qq:nickname";

            /// <summary>
            /// 大小为30×30像素的QQ空间头像URL。
            /// </summary>
            public const string PictureUrl = "urn:qq:picture";

            /// <summary>
            /// 大小为50×50像素的QQ空间头像URL。
            /// </summary>
            public const string PictureMediumUrl = "urn:qq:picture_medium";

            /// <summary>
            /// 大小为100×100像素的QQ空间头像URL。
            /// </summary>
            public const string PictureFullUrl = "urn:qq:picture_full";

            /// <summary>
            /// 大小为40×40像素的QQ头像URL。
            /// </summary>
            public const string AvatarUrl = "urn:qq:avatar";

            /// <summary>
            /// 大小为100×100像素的QQ头像URL。需要注意，不是所有的用户都拥有QQ的100x100的头像，但40x40像素则是一定会有。
            /// </summary>
            public const string AvatarFullUrl = "urn:qq:avatar_full";

            /// <summary>
            /// 标识用户是否为黄钻用户（0：不是；1：是）。
            /// </summary>
            public const string IsYellowVip = "urn:qq:is_yellow_vip";

            /// <summary>
            /// 标识用户是否为黄钻用户（0：不是；1：是）
            /// </summary>
            public const string Vip = "urn:qq:vip";

            /// <summary>
            /// 黄钻等级
            /// </summary>
            public const string YellowVipLevel = "urn:qq:yellow_vip_level";

            /// <summary>
            /// 黄钻等级
            /// </summary>
            public const string Level = "urn:qq:level";

            /// <summary>
            /// 标识是否为年费黄钻用户（0：不是； 1：是）
            /// </summary>
            public const string IsYellowYearVip = "urn:qq:is_yellow_year_vip";
        }
    }
}