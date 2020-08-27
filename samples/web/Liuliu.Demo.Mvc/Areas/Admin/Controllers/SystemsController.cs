using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    public class SystemsController : AdminControllerBase
    {
        [Description("系统设置")]
        public IActionResult Settings()
        {
            return View();
        }

        [Description("操作审计")]
        public IActionResult AuditOperations()
        {
            return View();
        }

        [Description("数据审计")]
        public IActionResult AuditEntities()
        {
            return View();
        }

        [Description("模块包管理")]
        public IActionResult Packs()
        {
            return View();
        }
    }
}
