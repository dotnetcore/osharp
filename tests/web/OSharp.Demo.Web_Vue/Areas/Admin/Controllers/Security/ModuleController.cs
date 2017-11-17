using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.UI;
using OSharp.Demo.Security.Dtos;


namespace OSharp.Demo.Web.Areas.Admin.Controllers
{
    [Description("管理-模块信息")]
    [Area("Admin")]
    [Route("api/[area]/[controller]/[action]")]
    public class ModuleController : Controller
    {
        [Description("读取")]
        public IActionResult Read()
        {
            return Json(new AjaxResult("数据读取功能尚未实现"));
        }

        [HttpPost]
        [Description("新增")]
        public async Task<IActionResult> Create(ModuleInputDto[] dtos)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Description("更新")]
        public async Task<IActionResult> Update(ModuleInputDto[] dtos)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Description("删除")]
        public async Task<IActionResult> Delete(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}
