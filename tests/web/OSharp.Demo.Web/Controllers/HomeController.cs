using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSharp.Demo.Web.Models;

namespace OSharp.Demo.Web.Controllers
{
    [Description("网站-主页")]
    public class HomeController : Controller
    {
        [Description("主页")]
        public IActionResult Index()
        {
            return View();
        }

        [Description("关于")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Description("联系我们")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Description("错误")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
