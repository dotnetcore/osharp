using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Entity;
using OSharp.EventBuses;
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

            var userRepository = _provider.GetService<IRepository<User, int>>();
            sb.AppendLine($"IRepository<User, int>： => {userRepository.GetHashCode()}");
            //sb.AppendLine($"IRepository<User, int>.DbContext： => {userRepository.DbContext.GetHashCode()}");

            userRepository = _provider.GetService<IRepository<User, int>>();
            sb.AppendLine($"IRepository<User, int>： => {userRepository.GetHashCode()}");
            //sb.AppendLine($"IRepository<User, int>.DbContext： => {userRepository.DbContext.GetHashCode()}");

            var roleRepository = _provider.GetService<IRepository<Role, int>>();
            sb.AppendLine($"IRepository<Role, int>： => {roleRepository.GetHashCode()}");
            //sb.AppendLine($"IRepository<Role, int>.DbContext： => {roleRepository.DbContext.GetHashCode()}");

            sb.AppendLine($"IIdentityContract: => {_provider.GetService<IIdentityContract>().GetHashCode()}");
            sb.AppendLine($"IIdentityContract: => {_provider.GetService<IIdentityContract>().GetHashCode()}");
            sb.AppendLine($"IEntityTypeFinder: => {_provider.GetService<IEntityTypeFinder>().GetHashCode()}");

            sb.AppendLine($"ServiceLocator.Instance: => {ServiceLocator.Instance.GetHashCode()}");

            sb.AppendLine($"_provider.GetService<IHttpContextAccessor>: => {_provider.GetService<IHttpContextAccessor>().GetHashCode()}");
            sb.AppendLine($"ServiceLocator.GetService<IHttpContextAccessor>: => {ServiceLocator.Instance.GetService<IHttpContextAccessor>().GetHashCode()}");

            sb.AppendLine($"_provider.GetService<IUnitOfWork>(): => {_provider.GetService<IUnitOfWork>().GetHashCode()}");
            sb.AppendLine($"ServiceLocator.GetScopedService<IUnitOfWork>: => {ServiceLocator.Instance.GetService<IUnitOfWork>().GetHashCode()}");

            sb.AppendLine($"_provider.GetService<IEventBus>(): => {_provider.GetService<IEventBus>().GetHashCode()}");
            sb.AppendLine($"EventBus.Default: => {EventBus.Default.GetHashCode()}");

            sb.AppendLine($"用户数量：{userRepository.Query().Count()}");

            return Content(sb.ToString());
        }

        public IActionResult AllServices([FromServices]IServiceProvider provider)
        {
            var services = ServiceLocator.Instance.GetServiceDescriptors();
            var sb = new StringBuilder();
            foreach (var item in services.Where(m => true)//m.ImplementationType != null && !m.ImplementationType.FullName.StartsWith("System") && !m.ImplementationType.FullName.StartsWith("Microsoft"))
                )//.OrderBy(m => m.Lifetime)).ThenBy(m => m.ImplementationType.FullName))
            {
                string line = $"{item.ImplementationType?.FullName}\t{item.ServiceType.FullName}\t{item.Lifetime}";
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

        public async Task<IActionResult> Test()
        {
            StringBuilder sb = new StringBuilder();
            string token = DateTime.Now.ToString("HHmmss");
            UserManager<User> userManager = _provider.GetService<UserManager<User>>();
            User user = new User() { UserName = $"gmf520{token}", Email = $"gmf520{token}@yeah.net" };
            IdentityResult result = await userManager.CreateAsync(user);
            sb.AppendLine(result.ToJsonString());

            RoleManager<Role> roleManager = _provider.GetService<RoleManager<Role>>();
            Role role = new Role() { Name = $"角色{token}" };
            result = await roleManager.CreateAsync(role);
            sb.AppendLine(result.ToJsonString());

            result = await userManager.AddToRoleAsync(user, role.Name);
            sb.AppendLine(result.ToJsonString());

            return Content(sb.ToString());
        }

        public void EventBusTest()
        {
            EventBus.Default.Trigger(this.GetType().Name, new TestEventData() { Name = "郭老板" });
        }
    }


    public class TestEventData : EventData
    {
        public string Name { get; set; }
    }


    public class TestEventHandler : IEventHandler<TestEventData>
    {
        private readonly ILogger<TestEventHandler> _logger;

        public TestEventHandler(ILogger<TestEventHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public void HandleEvent(TestEventData eventData)
        {
            _logger.LogInformation($"Hello {eventData.Name}, this is {eventData.EventSource}");
        }
    }
}
