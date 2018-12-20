
using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;


namespace Liuliu.Demo.Web.Controllers
{
    public class Test2Controller : Controller
    {
        [HttpGet]
        [Description("功能描述")]
        public IActionResult GetString()
        {
            return Content("String from Test2");
        }
    }
}
