using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OSharp.Demo.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Description("管理-主页")]
    public class HomeController : Controller
    {
        [Description("首页")]
        public IActionResult Index()
        {
            return Content("Admin-Home-Index");
        }
    }
}