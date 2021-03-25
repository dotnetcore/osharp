using Xunit;
using OSharp.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore.Tests.Mvc;
using OSharp.AspNetCore.UI;
using OSharp.Authorization.Functions;


namespace OSharp.AspNetCore.Mvc.Tests
{
    public class MvcFunctionHandlerTests
    {
        private IFunctionHandler GetHandler()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IFunctionHandler, MvcFunctionHandler>();
            IServiceProvider provider = services.BuildServiceProvider();
            IFunctionHandler handler = provider.GetRequiredService<IFunctionHandler>();
            return handler;
        }

        [Fact()]
        public void GetAllFunctionTypesTest()
        {
            IFunctionHandler handler = GetHandler();
            Type[] types = handler.GetAllFunctionTypes();
            Assert.NotEmpty(types);
        }

        [Fact()]
        public void GetMethodInfosTest()
        {
            IFunctionHandler handler = GetHandler();

            Type type = typeof(PublicController);
            MethodInfo[] methods = handler.GetMethodInfos(type);
            Assert.Contains(methods, m =>m.Name == "Read");

            type = typeof(Internal1Controller);
            methods = handler.GetMethodInfos(type);
            Assert.Contains(methods, m => m.Name == "Read1");

            type = typeof(Internal2Controller);
            methods = handler.GetMethodInfos(type);
            Assert.Contains(methods, m => m.Name == "Read2");
            Assert.Contains(methods, m => m.Name == "Index");

            type = typeof(AbstractController);
            methods = handler.GetMethodInfos(type);
            Assert.Contains(methods, m => m.Name == "Index");

            type = typeof(AttributeFunction);
            methods = handler.GetMethodInfos(type);
            Assert.Contains(methods, m => m.Name == "Index");

            type = typeof(NamingController);
            methods = handler.GetMethodInfos(type);
            Assert.Contains(methods, m => m.Name == "Index");

            type = typeof(NamingController2);
            methods = handler.GetMethodInfos(type);
            Assert.Empty(methods);

            type = typeof(AttributeNonController);
            methods = handler.GetMethodInfos(type);
            Assert.Empty(methods);
        }
    }
}