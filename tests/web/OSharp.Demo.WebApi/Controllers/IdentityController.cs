// -----------------------------------------------------------------------
//  <copyright file="IdentityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-29 2:44</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Http;
using OSharp.AspNetCore.Infrastructure;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Identity;


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

        [Description("用户昵称是否存在")]
        public IActionResult CheckNickNameExists(string nickName)
        {
            bool exists = _userManager.Users.Any(m => m.NickName == nickName);
            return Json(exists);
        }

        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("用户注册")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult("提交信息验证失败", AjaxResultType.Error));
            }
            if (!VerifyCodeHandler.CheckCode(dto.VerifyCode, true))
            {
                return Json(new AjaxResult("验证码错误，请刷新重试", AjaxResultType.Error));
            }
            dto.RegisterIp = HttpContext.GetClientIp();

            OperationResult result = await _identityContract.Register(dto);

            return Json(result.ToAjaxResult());
        }
        
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("用户登录")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult("提交信息验证失败", AjaxResultType.Error));
            }
            if (!VerifyCodeHandler.CheckCode(dto.VerifyCode, true))
            {
                return Json(new AjaxResult("验证码错误，请刷新重试", AjaxResultType.Error));
            }

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
            var data = new
            {
                User = new {UserId = user.Id,user.UserName,user.Email,UserRole = roles.ExpandAndToString()},
                SessionId = HttpContext.Session.Id
            };
            return Json(new AjaxResult("登录成功", AjaxResultType.Success, data));
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
                return Json(new AjaxResult($"编号为“{dto.UserId}”的用户信息不存在", AjaxResultType.Error));
            }
            IdentityResult result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            return Json(result.ToOperationResult().ToAjaxResult());
        }

        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("重置密码")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            User user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null)
            {
                return Json(new AjaxResult($"编号为“{dto.UserId}”的用户信息不存在", AjaxResultType.Error));
            }
            IdentityResult result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

            return Json(result.ToOperationResult().ToAjaxResult());
        }
    }
}