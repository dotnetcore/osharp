using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.Demo.Web.Controllers
{
    public class TestsController : Controller
    {
        private readonly IServiceProvider _provider;

        public TestsController(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Resolve()
        {
            StringBuilder sb = new StringBuilder();

            var repository = _provider.GetService<IRepository<User, int>>();
            sb.AppendLine($"IRepository<User, int>： => {repository.GetHashCode()}");
            sb.AppendLine($"IRepository<User, int>.DbContext： => {repository.DbContext.GetHashCode()}");

            repository = _provider.GetService<IRepository<User, int>>();
            sb.AppendLine($"IRepository<User, int>： => {repository.GetHashCode()}");
            sb.AppendLine($"IRepository<User, int>.DbContext： => {repository.DbContext.GetHashCode()}");

            sb.AppendLine($"IIdentityContract: => {_provider.GetService<IIdentityContract>().GetHashCode()}");
            sb.AppendLine($"IIdentityContract: => {_provider.GetService<IIdentityContract>().GetHashCode()}");
            sb.AppendLine($"IEntityTypeFinder: => {_provider.GetService<IEntityTypeFinder>().GetHashCode()}");

            return Content(sb.ToString());
        }

        public IActionResult AllServices([FromServices]IServiceProvider provider)
        {
            var services = Startup.Services;
            var sb = new StringBuilder();
            foreach (var item in services.Where(m => !m.ServiceType.FullName.StartsWith("System") && !m.ServiceType.FullName.StartsWith("Microsoft"))
                .OrderBy(m => m.Lifetime).ThenBy(m => m.ImplementationType.FullName))
            {
                string line = $"{item.ImplementationType.FullName}\t{item.ServiceType.FullName}\t{item.Lifetime}";
                try
                {
                    var code = provider.GetServices(item.ServiceType).First().GetHashCode();
                    line += $"\tHashCode:{code}";
                }
                catch (Exception)
                { }
                sb.AppendLine(line);
            }

            return Content(sb.ToString());
        }

        public IActionResult Mapper()
        {
            StringBuilder sb = new StringBuilder();

            UserInputDto dto = new UserInputDto()
            {
                Id = 1,
                UserName = "lao guo",
                NickName = "郭老板"
            };
            sb.AppendLine($"UserInputDto：{dto.ToJsonString()}");

            User user = dto.MapTo<User>();
            sb.AppendLine($"User: {user.ToJsonString()}");

            return Content(sb.ToString());
        }
    }
}
