using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Modules;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-认证")]
    [ModuleInfo(Order = 1)]
    public class IdentityController : SiteControllerBase
    {
        [Description("登录页面")]
        public IActionResult Login()
        {
            return View();
        }

    }
}
