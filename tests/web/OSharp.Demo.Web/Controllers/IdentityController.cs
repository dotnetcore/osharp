// -----------------------------------------------------------------------
//  <copyright file="IdentityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 14:11</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Entities;
using OSharp.Entity;

namespace OSharp.Demo.Web.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IIdentityContract _identityContract;
        private readonly IServiceProvider _provider;

        public IdentityController(IIdentityContract identityContract, IServiceProvider provider)
        {
            _identityContract = identityContract;
            _provider = provider;
        }

        public IActionResult Index()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"IEntityConfigurationAssemblyFinder => {_provider.GetService<IEntityConfigurationAssemblyFinder>().GetType()}");
            sb.AppendLine($"IEntityConfigurationAssemblyFinder => {_provider.GetService<IEntityConfigurationAssemblyFinder>().GetHashCode()}");
            sb.AppendLine($"IEntityConfigurationAssemblyFinder => {_provider.GetService<IEntityConfigurationAssemblyFinder>().GetHashCode()}");

            sb.AppendLine($"IIdentityContract => {_identityContract.GetType()}");
            sb.AppendLine($"IUnitOfWork => {_provider.GetService(typeof(IUnitOfWork)).GetType()}");
            sb.AppendLine($"IUnitOfWork => {_provider.GetService(typeof(IUnitOfWork)).GetHashCode()}");
            sb.AppendLine($"IUnitOfWork => {_provider.GetService(typeof(IUnitOfWork)).GetHashCode()}");
            sb.AppendLine($"IRepository<User,int> => {_provider.GetService(typeof(IRepository<User, int>)).GetType()}");
            sb.AppendLine($"当前用户数量： {_identityContract.Users().Count()}");

            return Content(sb.ToString());
        }
    }
}