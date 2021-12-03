using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Liuliu.Demo.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.Extensions.DependencyModel;

using OSharp.AspNetCore.UI;
using OSharp.Authorization.Modules;
using OSharp.Collections;
using OSharp.Core.Options;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Entity.DynamicProxy;
using OSharp.Identity;

using StackExchange.Profiling.Internal;


namespace Liuliu.Demo.Web.Controllers
{
    public class Test2Controller : SiteApiControllerBase
    {
        private readonly DefaultDbContext _dbContext;
        private readonly RoleManager<Role> _roleManager;

        public Test2Controller(DefaultDbContext dbContext, RoleManager<Role> roleManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Description("测试一下")]
        [Transactional]
        public virtual async Task<OperationResult<Role>> Test01(string name)
        {
            Role role = new Role() { Name = name, IsDefault = false };
            var result = await _roleManager.CreateAsync(role);
            OperationResult result2 = result.ToOperationResult();
            return new OperationResult<Role>(result2.ResultType, result2.Message, role);
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
