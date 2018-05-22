// -----------------------------------------------------------------------
//  <copyright file="IdentityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-29 2:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Http;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Data;
using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Identity;
using OSharp.Net;
using OSharp.Secutiry.Claims;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-身份认证")]
    public class IdentityController : ApiController
    {
        private readonly IIdentityContract _identityContract;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityController(IIdentityContract identityContract,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _identityContract = identityContract;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Description("用户名是否存在")]
        public IActionResult CheckUserNameExists(string userName)
        {
            bool exists = _userManager.Users.Any(m => m.NormalizedUserName == _userManager.NormalizeKey(userName));
            return Json(exists);
        }

        [Description("用户Email是否存在")]
        public IActionResult CheckEmailExists(string email)
        {
            bool exists = _userManager.Users.Any(m => m.NormalizeEmail == _userManager.NormalizeKey(email));
            return Json(exists);
        }

        [Description("用户Email是否不存在")]
        public IActionResult CheckEmailNotExists(string email)
        {
            bool exists = !_userManager.Users.Any(m => m.NormalizeEmail == _userManager.NormalizeKey(email));
            return Json(exists);
        }

        [Description("用户昵称是否存在")]
        public IActionResult CheckNickNameExists(string nickName)
        {
            bool exists = _userManager.Users.Any(m => m.NickName == nickName);
            return Json(exists);
        }

        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("用户注册")]
        public async Task<IActionResult> Register([FromBody]RegisterDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult("提交信息验证失败", AjaxResultType.Error));
            }
            //todo: 校验验证码

            dto.RegisterIp = HttpContext.GetClientIp();

            OperationResult<User> result = await _identityContract.Register(dto);

            if (result.Successed)
            {
                User user = result.Data;
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string url = $"{Request.Scheme}://{Request.Host}/#/identity/confirm-email?userId={user.Id}&code={code}";
                string body =
                    $"亲爱的用户 <strong>{user.NickName}</strong>[{user.UserName}]，您好！<br>"
                    + $"欢迎注册，激活邮箱请 <a href=\"{url}\" target=\"_blank\"><strong>点击这里</strong></a><br>"
                    + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
                    + $"{url}<br>"
                    + $"祝您使用愉快！";
                await SendMailAsync(user.Email, "柳柳软件 注册邮箱激活邮件", body);
            }

            return Json(result.ToAjaxResult());
        }

        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("用户登录")]
        public async Task<IActionResult> Login([FromBody]LoginDto dto)
        {
            Check.NotNull(dto, nameof(dto));
            throw new NotImplementedException();
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult("提交信息验证失败", AjaxResultType.Error));
            }
            //todo: 校验验证码

            dto.Ip = HttpContext.GetClientIp();
            dto.UserAgent = Request.Headers["User-Agent"].FirstOrDefault();

            OperationResult<User> result = await _identityContract.Login(dto);
            if (!result.Successed)
            {
                return Json(result.ToAjaxResult());
            }
            User user = result.Data;
            await _signInManager.SignInAsync(user, dto.Remember);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            var data = new { UserId = user.Id, user.UserName, user.NickName, user.Email, Roles = roles };
            return Json(new AjaxResult("登录成功", AjaxResultType.Success, data));
        }

        [Description("用户信息")]
        public IActionResult UserProfile()
        {
            if (!User.Identity.IsAuthenticated || !(User.Identity is ClaimsIdentity identity))
            {
                return Json(null);
            }
            int userId = identity.GetUserId<int>();
            string userName = identity.GetUserName();
            string email = identity.GetEmail();
            string nickName = identity.GetNickName();
            string[] roles = identity.GetRoles().ToArray();
            var data = new
            {
                User = new { UserId = userId, UserName = userName, NickName = nickName, Email = email, Roles = roles },
                SessionId = HttpContext.Session.Id
            };
            return Json(data);
        }

        [HttpPost]
        [Description("激活邮箱")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult("邮箱激活失败：参数不正确", AjaxResultType.Error));
            }
            User user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null)
            {
                return Json(new AjaxResult("注册邮箱激活失败：用户不存在", AjaxResultType.Error));
            }
            if (user.EmailConfirmed)
            {
                return Json(new AjaxResult("注册邮箱已激活，操作取消", AjaxResultType.Info));
            }
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, dto.Code);
            return Json(result.ToOperationResult().ToAjaxResult());
        }

        [HttpPost]
        [Description("发送激活Email邮件")]
        public async Task<IActionResult> SendConfirmMail([FromBody]SendMailDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult("提交信息验证失败", AjaxResultType.Error));
            }

            User user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Json(new AjaxResult("发送激活邮件失败：用户不存在", AjaxResultType.Error));
            }
            if (user.EmailConfirmed)
            {
                return Json(new AjaxResult("Email已激活，无需再次激活", AjaxResultType.Info));
            }
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string url = $"{Request.Scheme}://{Request.Host}/#/identity/confirm-email?userId={user.Id}&code={code}";
            string body =
                $"亲爱的用户 <strong>{user.NickName}</strong>[{user.UserName}]，你好！<br>"
                + $"欢迎注册，激活邮箱请 <a href=\"{url}\" target=\"_blank\"><strong>点击这里</strong></a><br>"
                + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
                + $"{url}<br>"
                + $"祝您使用愉快！";
            await SendMailAsync(user.Email, "柳柳软件 注册邮箱激活邮件", body);
            return Json(new AjaxResult("激活Email邮件发送成功"));
        }

        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("修改密码")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            User user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null)
            {
                return Json(new AjaxResult($"用户不存在", AjaxResultType.Error));
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            return Json(result.ToOperationResult().ToAjaxResult());
        }

        [HttpPost]
        [Description("发送重置邮件")]
        public async Task<IActionResult> SendResetPasswordMail([FromBody]SendMailDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult("提交数据验证失败", AjaxResultType.Error));
            }
            //todo: 校验验证码
            User user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Json(new AjaxResult("用户不存在", AjaxResultType.Error));
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IEmailSender sender = ServiceLocator.Instance.GetService<IEmailSender>();
            string url = $"{Request.Scheme}://{Request.Host}/#/identity/reset-password?userId={user.Id}&token={token}";
            string body = $"亲爱的用户 <strong>{user.NickName}</strong>[{user.UserName}]，您好！<br>"
                + $"欢迎使用柳柳软件账户密码重置功能，请 <a href=\"{url}\" target=\"_blank\"><strong>点击这里</strong></a><br>"
                + $"如果上面的链接无法点击，您可以复制以下地址，并粘贴到浏览器的地址栏中打开。<br>"
                + $"{url}<br>"
                + $"祝您使用愉快！";
            await sender.SendEmailAsync(user.Email, "柳柳软件 重置密码邮件", body);
            return Json(new AjaxResult("密码重置邮件发送成功"));
        }

        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("重置登录密码")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            User user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null)
            {
                return Json(new AjaxResult($"用户不存在", AjaxResultType.Error));
            }
            IdentityResult result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

            return Json(result.ToOperationResult().ToAjaxResult());
        }

        private static async Task SendMailAsync(string email, string subject, string body)
        {
            IEmailSender sender = ServiceLocator.Instance.GetService<IEmailSender>();
            await sender.SendEmailAsync(email, subject, body);
        }
    }
}