using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.AspNetCore.UI;
using OSharp.Authorization.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Hosting.Identity;
using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-认证")]
    [ModuleInfo(Order = 1)]
    public class IdentityController : SiteControllerBase
    {
        private readonly IIdentityContract _identityContract;
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// 初始化一个<see cref="IdentityController"/>类型的新实例
        /// </summary>
        public IdentityController(IIdentityContract identityContract,
            SignInManager<User> signInManager)
        {
            _identityContract = identityContract;
            _signInManager = signInManager;
        }

        [ModuleInfo]
        [Description("用户登录")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            string returnUrl = GetReturnUrl();
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [ModuleInfo]
        [Description("用户登录")]
        public async Task<AjaxResult> Login(LoginDto dto)
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
            IUnitOfWork unitOfWork = HttpContext.RequestServices.GetUnitOfWork<User, int>();
            unitOfWork.Commit();

            if (!result.Succeeded)
            {
                return result.ToAjaxResult();
            }

            User user = result.Data;
            await _signInManager.SignInAsync(user, dto.Remember);
            return new AjaxResult("登录成功");
        }

        private string GetReturnUrl()
        {
            string[] noBackUrls = { "identity/login", "identity/register" };
            string returnUrl = Request.Query["returnUrl"];
            if (returnUrl.IsNullOrEmpty())
            {
                returnUrl = Url.Action("Index", "Home", new { area = "" });
            }

            if (returnUrl != null && noBackUrls.Any(m => returnUrl.Contains(m)))
            {
                returnUrl = Url.Action("Index", "Home", new { area = "" });
            }

            return returnUrl;
        }
    }
}
