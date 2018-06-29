// -----------------------------------------------------------------------
//  <copyright file="IdentityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Liuliu.Demo.Identity;
using Liuliu.Demo.Identity.Dtos;
using Liuliu.Demo.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Collections;
using OSharp.Core;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Identity;
using OSharp.Net;
using OSharp.Security.JwtBearer;
using OSharp.Secutiry.Claims;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-认证")]
    [ModuleInfo(Order = 1)]
    public class IdentityController : ApiController
    {
        private readonly IIdentityContract _identityContract;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public IdentityController(IIdentityContract identityContract,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _identityContract = identityContract;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// 用户名是否存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>是否存在</returns>
        [HttpGet]
        [Description("用户名是否存在")]
        public bool CheckUserNameExists(string userName)
        {
            bool exists = _userManager.Users.Any(m => m.NormalizedUserName == _userManager.NormalizeKey(userName));
            return exists;
        }

        /// <summary>
        /// 用户Email是否存在
        /// </summary>
        /// <param name="email">电子邮箱</param>
        /// <returns>是否存在</returns>
        [HttpGet]
        [Description("用户Email是否存在")]
        public bool CheckEmailExists(string email)
        {
            bool exists = _userManager.Users.Any(m => m.NormalizeEmail == _userManager.NormalizeKey(email));
            return exists;
        }

        /// <summary>
        /// 用户昵称是否存在
        /// </summary>
        /// <param name="nickName">用户昵称</param>
        /// <returns>是否存在</returns>
        [HttpGet]
        [Description("用户昵称是否存在")]
        public bool CheckNickNameExists(string nickName)
        {
            bool exists = _userManager.Users.Any(m => m.NickName == nickName);
            return exists;
        }

        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <param name="dto">注册信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [DependOnFunction("CheckUserNameExists")]
        [DependOnFunction("CheckEmailExists")]
        [DependOnFunction("CheckNickNameExists")]
        [Description("用户注册")]
        public async Task<AjaxResult> Register([FromBody] RegisterDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
            }
            if (!VerifyCodeHandler.CheckCode(dto.VerifyCode, dto.VerifyCodeId))
            {
                return new AjaxResult("验证码错误，请刷新重试", AjaxResultType.Error);
            }

            dto.RegisterIp = HttpContext.GetClientIp();

            OperationResult<User> result = await _identityContract.Register(dto);

            if (result.Successed)
            {
                User user = result.Data;
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = UrlBase64ReplaceChar(code);
                string url = $"{Request.Scheme}://{Request.Host}/#/identity/confirm-email?userId={user.Id}&code={code}";
                string body =
                    $"亲爱的用户 <strong>{user.NickName}</strong>[{user.UserName}]，您好！<br>"
                    + $"欢迎注册，激活邮箱请 <a href=\"{url}\" target=\"_blank\"><strong>点击这里</strong></a><br>"
                    + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
                    + $"{url}<br>"
                    + $"祝您使用愉快！";
                await SendMailAsync(user.Email, "柳柳软件 注册邮箱激活邮件", body);
            }

            return result.ToAjaxResult();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="dto">登录信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("用户登录")]
        public async Task<AjaxResult> Login([FromBody] LoginDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
            }
            //todo: 校验验证码

            dto.Ip = HttpContext.GetClientIp();
            dto.UserAgent = Request.Headers["User-Agent"].FirstOrDefault();

            OperationResult<User> result = await _identityContract.Login(dto);
            IUnitOfWork unitOfWork = ServiceLocator.Instance.GetService<IUnitOfWork>();
            unitOfWork.Commit();

            if (!result.Successed)
            {
                return result.ToAjaxResult();
            }
            User user = result.Data;
            await _signInManager.SignInAsync(user, dto.Remember);
            return new AjaxResult("登录成功");
        }

        /// <summary>
        /// Jwt登录
        /// </summary>
        /// <param name="dto">登录信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("JWT登录")]
        public async Task<AjaxResult> Jwtoken([FromBody] LoginDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
            }
            dto.Ip = HttpContext.GetClientIp();
            dto.UserAgent = Request.Headers["User-Agent"].FirstOrDefault();

            OperationResult<User> result = await _identityContract.Login(dto);
            IUnitOfWork unitOfWork = ServiceLocator.Instance.GetService<IUnitOfWork>();
            unitOfWork.Commit();

            if (!result.Successed)
            {
                return result.ToAjaxResult();
            }
            User user = result.Data;
            bool isAdmin = _identityContract.Roles.Any(m =>
                m.IsAdmin && _identityContract.UserRoles.Where(n => n.UserId == user.Id).Select(n => n.RoleId).Contains(m.Id));
            IList<string> roles = await _userManager.GetRolesAsync(user);

            //生成Token
            Claim[] claims =
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.NickName ?? user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.HeadImage, user.HeadImg ?? ""),
                new Claim(JwtClaimTypes.SecurityStamp, user.SecurityStamp),
                new Claim(JwtClaimTypes.IsAdmin, isAdmin.ToLower()),
                new Claim(ClaimTypes.Role, roles.ExpandAndToString())
            };
            string token = JwtHelper.CreateToken(claims);
            return new AjaxResult("登录成功", AjaxResultType.Success, token);
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("用户登出")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        public async Task<AjaxResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new AjaxResult("用户登出成功");
            }
            int userId = User.Identity.GetUserId<int>();
            OperationResult result = await _identityContract.Logout(userId);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 激活邮箱
        /// </summary>
        /// <param name="dto">电子邮箱</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("激活邮箱")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        public async Task<AjaxResult> ConfirmEmail([FromBody] ConfirmEmailDto dto)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult("邮箱激活失败：参数不正确", AjaxResultType.Error);
            }
            User user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null)
            {
                return new AjaxResult("注册邮箱激活失败：用户不存在", AjaxResultType.Error);
            }
            if (user.EmailConfirmed)
            {
                return new AjaxResult("注册邮箱已激活，操作取消", AjaxResultType.Info);
            }
            string code = UrlBase64ReplaceChar(dto.Code);
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
            return result.ToOperationResult().ToAjaxResult();
        }

        /// <summary>
        /// 发送激活注册邮件
        /// </summary>
        /// <param name="dto">激活邮箱信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("CheckEmailExists")]
        [Description("发送激活注册邮件")]
        public async Task<AjaxResult> SendConfirmMail([FromBody] SendMailDto dto)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
            }
            if (!VerifyCodeHandler.CheckCode(dto.VerifyCode, dto.VerifyCodeId))
            {
                return new AjaxResult("验证码错误，请刷新重试", AjaxResultType.Error);
            }
            User user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new AjaxResult("发送激活邮件失败：用户不存在", AjaxResultType.Error);
            }
            if (user.EmailConfirmed)
            {
                return new AjaxResult("Email已激活，无需再次激活", AjaxResultType.Info);
            }
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = UrlBase64ReplaceChar(code);
            string url = $"{Request.Scheme}://{Request.Host}/#/identity/confirm-email?userId={user.Id}&code={code}";
            string body =
                $"亲爱的用户 <strong>{user.NickName}</strong>[{user.UserName}]，你好！<br>"
                + $"欢迎注册，激活邮箱请 <a href=\"{url}\" target=\"_blank\"><strong>点击这里</strong></a><br>"
                + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
                + $"{url}<br>"
                + $"祝您使用愉快！";
            await SendMailAsync(user.Email, "柳柳软件 注册邮箱激活邮件", body);
            return new AjaxResult("激活Email邮件发送成功");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dto">修改密码信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [Logined]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("修改密码")]
        public async Task<AjaxResult> ChangePassword(ChangePasswordDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            User user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null)
            {
                return new AjaxResult($"用户不存在", AjaxResultType.Error);
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            return result.ToOperationResult().ToAjaxResult();
        }

        /// <summary>
        /// 发送重置密码邮件
        /// </summary>
        /// <param name="dto">发送邮件信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("CheckEmailExists")]
        [Description("发送重置密码邮件")]
        public async Task<AjaxResult> SendResetPasswordMail([FromBody] SendMailDto dto)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxResult("提交数据验证失败", AjaxResultType.Error);
            }
            if (!VerifyCodeHandler.CheckCode(dto.VerifyCode, dto.VerifyCodeId))
            {
                return new AjaxResult("验证码错误，请刷新重试", AjaxResultType.Error);
            }

            User user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new AjaxResult("用户不存在", AjaxResultType.Error);
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = UrlBase64ReplaceChar(token);
            IEmailSender sender = ServiceLocator.Instance.GetService<IEmailSender>();
            string url = $"{Request.Scheme}://{Request.Host}/#/identity/reset-password?userId={user.Id}&token={token}";
            string body = $"亲爱的用户 <strong>{user.NickName}</strong>[{user.UserName}]，您好！<br>"
                + $"欢迎使用柳柳软件账户密码重置功能，请 <a href=\"{url}\" target=\"_blank\"><strong>点击这里</strong></a><br>"
                + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
                + $"{url}<br>"
                + $"祝您使用愉快！";
            await sender.SendEmailAsync(user.Email, "柳柳软件 重置密码邮件", body);
            return new AjaxResult("密码重置邮件发送成功");
        }

        /// <summary>
        /// 重置登录密码
        /// </summary>
        /// <param name="dto">重置密码信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("重置登录密码")]
        public async Task<AjaxResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            User user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null)
            {
                return new AjaxResult($"用户不存在", AjaxResultType.Error);
            }
            string token = UrlBase64ReplaceChar(dto.Token);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

            return result.ToOperationResult().ToAjaxResult();
        }

        private static async Task SendMailAsync(string email, string subject, string body)
        {
            IEmailSender sender = ServiceLocator.Instance.GetService<IEmailSender>();
            await sender.SendEmailAsync(email, subject, body);
        }

        private static string UrlBase64ReplaceChar(string source)
        {
            if (source.Contains('+') || source.Contains('/'))
            {
                return source.Replace('+', '-').Replace('/', '_');
            }
            if (source.Contains('-') || source.Contains('_'))
            {
                return source.Replace('-', '+').Replace('_', '/');
            }
            return source;
        }
    }
}