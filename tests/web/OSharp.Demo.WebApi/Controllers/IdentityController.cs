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


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-身份认证")]
    public class IdentityController : Controller
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

        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("用户注册")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            
            return Json("");
        }


        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("用户登录")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult("提交信息验证失败", AjaxResultType.Error));
            }
            if (!VerifyCodeHandler.CheckCode(dto.VerifyCode, true))
            {
                return Json(new AjaxResult("验证码错误，请刷新重试"));
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
    }
}