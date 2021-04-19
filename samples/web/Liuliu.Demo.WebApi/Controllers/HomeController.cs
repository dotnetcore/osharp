using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization.Modules;
using OSharp.Core.Systems;
using OSharp.Hosting.Apis.Controllers;
using OSharp.Json;


namespace Liuliu.Demo.Web.Controllers
{
    public class HomeController : SiteApiControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="SiteApiControllerBase"/>类型的新实例
        /// </summary>
        public HomeController(IServiceProvider provider)
            : base(provider)
        { }
        
    }
}
