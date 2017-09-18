using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OSharp.Demo.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Description("管理-首页")]
    public class HomeController : Controller
    {
        [Description("主页")]
        public IActionResult Index()
        {
            return View();
        }

        [Description("测试1")]
        public IActionResult Test1()
        {
            return Content("Admin.Home.Test1");
        }

        [Description("测试2")]
        public string Test2()
        {
            return "Admin.Home.Test2";
        }
    }
}