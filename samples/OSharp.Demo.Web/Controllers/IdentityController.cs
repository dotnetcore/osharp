// -----------------------------------------------------------------------
//  <copyright file="IdentityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 14:11</last-date>
// -----------------------------------------------------------------------

using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using OSharp.Demo.Identity;

namespace OSharp.Demo.Web.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IIdentityContract _identityContract;

        public IdentityController(IIdentityContract identityContract)
        {
            _identityContract = identityContract;
        }

        public IActionResult Index()
        {
            var userQuery = _identityContract.Users();

            return Content($"用户数：{userQuery.Count()}");
        }
    }
}