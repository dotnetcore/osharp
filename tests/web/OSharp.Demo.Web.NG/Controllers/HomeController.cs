using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Mapping;


namespace OSharp.Demo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityContract _identityContract;

        public HomeController(IIdentityContract identityContract)
        {
            _identityContract = identityContract;
        }

        public IActionResult Index()
        {
            ViewBag.Data = _identityContract.GetType().FullName;
            return View();
        }
    }
}