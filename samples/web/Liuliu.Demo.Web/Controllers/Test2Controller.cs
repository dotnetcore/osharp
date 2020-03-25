using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Authorization.Modules;
using OSharp.Collections;
using OSharp.Core.Options;
using OSharp.Core.Systems;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Redis;

using StackExchange.Profiling.Internal;


namespace Liuliu.Demo.Web.Controllers
{
    public class Test2Controller : SiteApiController
    {
        private readonly DefaultDbContext _dbContext;

        public Test2Controller(DefaultDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Description("测试一下")]
        public IActionResult Test01()
        {
            RedisClient redis = new RedisClient();
            redis.StringSet("Test:Key001", "value001", TimeSpan.FromSeconds(20));
            return Content(redis.StringGet("Test:Key001"));
        }

        /// <summary>
        /// 功能注释
        /// </summary>
        /// <returns>返回数据</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("测试一下")]
        public string Test02()
        {
            var val = AppSettingsReader.GetValue<string>("OSharp:DbContexts:SqlServer:DbContextTypeName");
            return val.ToJson();

            return DependencyContext.Default.CompileLibraries.Select(m => $"{m.Name},{m.Version}").ExpandAndToString("\r\n");
        }
    }
}
