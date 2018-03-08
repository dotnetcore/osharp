using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Mapping;


namespace OSharp.Demo.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}