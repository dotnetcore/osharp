
using System.ComponentModel;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc;
using OSharp.Dependency;


namespace Liuliu.Demo.Web.Controllers
{
    public class Test2Controller : ApiController
    {
        private readonly ScopedDictionary _dictionary;

        /// <summary>
        /// 初始化一个<see cref="Test2Controller"/>类型的新实例
        /// </summary>
        public Test2Controller(ScopedDictionary dictionary)
        {
            _dictionary = dictionary;
        }

        [HttpGet]
        [Description("功能描述")]
        public IActionResult GetString()
        {
            ScopedDictionary dict = HttpContext.RequestServices.GetService<ScopedDictionary>();
            IVerifyCodeService codeService = HttpContext.RequestServices.GetService<IVerifyCodeService>();
            IVerifyCodeService[] codeServices = HttpContext.RequestServices.GetServices<IVerifyCodeService>().ToArray();
            return Content($"{_dictionary.Count} - {_dictionary.GetHashCode()} - {dict.GetHashCode()} - {codeService.GetHashCode()} - {codeServices.Length}");
        }
    }
}
