using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using OSharp.Collections;
using OSharp.Demo.Identity;
using OSharp.Entity;
using OSharp.Infrastructure;


namespace OSharp.Demo.Web.Controllers
{
    [Description("网站-主页")]
    public class HomeController : Controller
    {
        private readonly IRepository<Function, Guid> _functionRepository;

        public HomeController(IRepository<Function, Guid> functionRepository)
        {
            _functionRepository = functionRepository;
        }

        [Description("首页")]
        public IActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{_functionRepository.Query(m => !m.IsLocked).Select(m => new { m.Id, m.Name }).ExpandAndToString("\n")}");


            ViewBag.Content = sb.ToString();
            return View();
        }
    }
}