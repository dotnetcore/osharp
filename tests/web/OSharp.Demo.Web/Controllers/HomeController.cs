using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using OSharp.Demo.Identity;


namespace OSharp.Demo.Web.Controllers
{
    [Description("网站-主页")]
    public class HomeController : Controller
    {
        private readonly IIdentityContract _identityContract;

        public HomeController(IIdentityContract identityContract)
        {
            _identityContract = identityContract;
        }

        [Description("首页")]
        public IActionResult Index()
        {
            return View();
        }
    }
}