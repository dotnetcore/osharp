// -----------------------------------------------------------------------
//  <copyright file="IdentityErrorDescriberZhHans.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-26 16:28</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

using OSharp.Extensions;


namespace OSharp.Identity
{
    /// <summary>
    /// Identity错误信息描述的zh-Hans本地化
    /// </summary>
    public class IdentityErrorDescriberZhHans : IdentityErrorDescriber
    {
        /// <summary>
        /// Returns the default <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" />.
        /// </summary>
        /// <returns>The default <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" />.</returns>
        public override IdentityError DefaultError()
        {
            IdentityError error = base.DefaultError();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a concurrency failure.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a concurrency failure.</returns>
        public override IdentityError ConcurrencyFailure()
        {
            IdentityError error = base.ConcurrencyFailure();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password mismatch.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password mismatch.</returns>
        public override IdentityError PasswordMismatch()
        {
            IdentityError error = base.PasswordMismatch();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating an invalid token.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating an invalid token.</returns>
        public override IdentityError InvalidToken()
        {
            IdentityError error = base.InvalidToken();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a recovery code was not redeemed.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a recovery code was not redeemed.</returns>
        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            IdentityError error = base.RecoveryCodeRedemptionFailed();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating an external login is already associated with an account.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating an external login is already associated with an account.</returns>
        public override IdentityError LoginAlreadyAssociated()
        {
            IdentityError error = base.LoginAlreadyAssociated();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified user <paramref name="userName" /> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified user <paramref name="userName" /> is invalid.</returns>
        public override IdentityError InvalidUserName(string userName)
        {
            IdentityError error = base.InvalidUserName(userName);
            error.Description = DescriptionDict[error.Code].FormatWith(userName);
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is invalid.</returns>
        public override IdentityError InvalidEmail(string email)
        {
            IdentityError error = base.InvalidEmail(email);
            error.Description = DescriptionDict[error.Code].FormatWith(email);
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="userName" /> already exists.
        /// </summary>
        /// <param name="userName">The user name that already exists.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="userName" /> already exists.</returns>
        public override IdentityError DuplicateUserName(string userName)
        {
            IdentityError error = base.DuplicateUserName(userName);
            error.Description = DescriptionDict[error.Code].FormatWith(userName);
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is already associated with an account.
        /// </summary>
        /// <param name="email">The email that is already associated with an account.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is already associated with an account.</returns>
        public override IdentityError DuplicateEmail(string email)
        {
            IdentityError error = base.DuplicateEmail(email);
            error.Description = DescriptionDict[error.Code].FormatWith(email);
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="role" /> name is invalid.
        /// </summary>
        /// <param name="role">The invalid role.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specific role <paramref name="role" /> name is invalid.</returns>
        public override IdentityError InvalidRoleName(string role)
        {
            IdentityError error = base.InvalidRoleName(role);
            error.Description = DescriptionDict[error.Code].FormatWith(role);
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="role" /> name already exists.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specific role <paramref name="role" /> name already exists.</returns>
        public override IdentityError DuplicateRoleName(string role)
        {
            IdentityError error = base.DuplicateRoleName(role);
            error.Description = DescriptionDict[error.Code].FormatWith(role);
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user already has a password.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user already has a password.</returns>
        public override IdentityError UserAlreadyHasPassword()
        {
            IdentityError error = base.UserAlreadyHasPassword();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating user lockout is not enabled.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating user lockout is not enabled.</returns>
        public override IdentityError UserLockoutNotEnabled()
        {
            IdentityError error = base.UserLockoutNotEnabled();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user is already in the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user is already in the specified <paramref name="role" />.</returns>
        public override IdentityError UserAlreadyInRole(string role)
        {
            IdentityError error = base.UserAlreadyInRole(role);
            error.Description = DescriptionDict[error.Code].FormatWith(role);
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user is not in the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user is not in the specified <paramref name="role" />.</returns>
        public override IdentityError UserNotInRole(string role)
        {
            IdentityError error = base.UserNotInRole(role);
            error.Description = DescriptionDict[error.Code].FormatWith(role);
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password of the specified <paramref name="length" /> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password of the specified <paramref name="length" /> does not meet the minimum length requirements.</returns>
        public override IdentityError PasswordTooShort(int length)
        {
            IdentityError error = base.PasswordTooShort(length);
            error.Description = DescriptionDict[error.Code].FormatWith(length);
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password does not meet the minimum number <paramref name="uniqueChars" /> of unique chars.
        /// </summary>
        /// <param name="uniqueChars">The number of different chars that must be used.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password does not meet the minimum number <paramref name="uniqueChars" /> of unique chars.</returns>
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            IdentityError error = base.PasswordRequiresUniqueChars(uniqueChars);
            error.Description = DescriptionDict[error.Code].FormatWith(uniqueChars);
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a non-alphanumeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a non-alphanumeric character.</returns>
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            IdentityError error = base.PasswordRequiresNonAlphanumeric();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a numeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a numeric character.</returns>
        public override IdentityError PasswordRequiresDigit()
        {
            IdentityError error = base.PasswordRequiresDigit();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a lower case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a lower case letter.</returns>
        public override IdentityError PasswordRequiresLower()
        {
            IdentityError error = base.PasswordRequiresLower();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain an upper case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain an upper case letter.</returns>
        public override IdentityError PasswordRequiresUpper()
        {
            IdentityError error = base.PasswordRequiresUpper();
            error.Description = DescriptionDict[error.Code];
            return error;
        }

        /// <summary>
        /// Identity错误信息字典
        /// </summary>
        public static readonly IDictionary<string, string> DescriptionDict = new Dictionary<string, string>()
        {
            { "ConcurrencyFailure", "乐观并发失败，对象已被修改。" },
            { "DefaultError", "发生了一个未知的错误。" },
            { "DuplicateEmail", "电子邮件“{0}”已被占用。" },
            { "DuplicateRoleName", "角色名称“{0}”已被占用。" },
            { "DuplicateUserName", "用户名“{0}”已被占用。" },
            { "InvalidEmail", "电子邮件“{0}”无效。" },
            { "InvalidManagerType", "类型{0}必须从{1}<{2}>派生。" },
            { "InvalidPasswordHasherCompatibilityMode", "提供的PasswordHasherCompatibilityMode无效。" },
            { "InvalidPasswordHasherIterationCount", "迭代计数必须是正整数。" },
            { "InvalidRoleName", "角色名称“{0}”无效。" },
            { "InvalidToken", "令牌Token无效。" },
            { "InvalidUserName", "用户名“{0}”无效，只能包含字母或数字。" },
            { "LoginAlreadyAssociated", "具有此登录名的用户已经存在。" },
            { "MustCallAddIdentity", "必须对服务集合调用AddIdEntity。" },
            { "NoPersonalDataProtector", "没有注册IPersonalDataProtector服务，这在ProtectPersonalData=true时是必需的。" },
            { "NoRoleType", "未指定RoleType，请尝试AddRoles<TRole>()。" },
            { "NoTokenProvider", "未注册名为“{1}”的IUserTwoFactorTokenProvider<{0}>。" },
            { "NullSecurityStamp", "用户安全标识不能为空。" },
            { "PasswordMismatch", "密码错误。" },
            { "PasswordRequiresDigit", "密码必须至少有一个数字('0'-'9')。" },
            { "PasswordRequiresLower", "密码必须至少有一个小写字母 ('a'-'z')。" },
            { "PasswordRequiresNonAlphanumeric", "密码必须至少有一个非字母数字字符。" },
            { "PasswordRequiresUniqueChars", "密码必须至少使用{0}不同的字符。" },
            { "PasswordRequiresUpper", "密码必须至少有一个大写字母 ('A'-'Z')。" },
            { "PasswordTooShort", "密码必须至少是{0}字符。" },
            { "RecoveryCodeRedemptionFailed", "恢复代码赎回失败。" },
            { "RoleNotFound", "角色{0}不存在。" },
            { "StoreNotIProtectedUserStore", "Store不实现IProtectedUserStore<TUser>，这在ProtectPersonalData=true时是必需的。" },
            { "StoreNotIQueryableRoleStore", "Store不实现IQueryableRoleStore<TRole>。" },
            { "StoreNotIQueryableUserStore", "Store不实现IQueryableUserStore<TUser>。" },
            { "StoreNotIRoleClaimStore", "Store不实现IRoleClaimStore<TRole>。" },
            { "StoreNotIUserAuthenticationTokenStore", "存储不实现IUserAuthenticationTokenStore<User>。" },
            { "StoreNotIUserAuthenticatorKeyStore", "存储不实现IUserAuthenticatorKeyStore<User>。" },
            { "StoreNotIUserClaimStore", "Store不实现IUserClaimStore<TUser>。" },
            { "StoreNotIUserConfirmationStore", "Stand不实现iUSER确认存储<TUser>。" },
            { "StoreNotIUserEmailStore", "Store不实现IUserEmailStore<TUser>。" },
            { "StoreNotIUserLockoutStore", "Store不实现IUserLockoutStore<TUser>。" },
            { "StoreNotIUserLoginStore", "Store不实现IUserLoginStore<TUser>。" },
            { "StoreNotIUserPasswordStore", "Store不实现IUserPasswordStore<TUser>。" },
            { "StoreNotIUserPhoneNumberStore", "存储不实现IUserPhoneNumberStore<TUser>。" },
            { "StoreNotIUserRoleStore", "Store不实现IUserRoleStore<TUser>。" },
            { "StoreNotIUserSecurityStampStore", "存储不实现IUserSecurityStampStore<TUser>。" },
            { "StoreNotIUserTwoFactorRecoveryCodeStore", "Store不实现IUserTwoFactorRecoveryCodeStore<TUser>。" },
            { "StoreNotIUserTwoFactorStore", "Store不实现IUserTwoFactorStore<TUser>。" },
            { "UserAlreadyHasPassword", "用户已经设置了密码。" },
            { "UserAlreadyInRole", "用户已经处于角色“{0}”中。" },
            { "UserLockedOut", "用户已被锁定，暂时无法登录。" },
            { "UserLockoutNotEnabled", "此用户未启用锁定。" },
            { "UserNameNotFound", "用户{0}不存在。" },
            { "UserNotInRole", "用户不在角色“{0}”中。" },
        };
    }
}