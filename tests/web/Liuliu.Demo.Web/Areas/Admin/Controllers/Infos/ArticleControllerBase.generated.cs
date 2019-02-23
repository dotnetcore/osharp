using System.ComponentModel;

using OSharp.Filter;
using OSharp.Core.Modules;

using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using System.Threading.Tasks;

namespace Liuliu.Demo.Web.Areas.Admin.Controllers.Infos
{
    [ModuleInfo(Position = "Infos", PositionName = "信息模块")]
    [Description("管理-文章信息")]
    public abstract class ArticleControllerBase : AdminApiController
    {
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public virtual PageData<PageCondition> Read(PageRequest request)
        {
            return null;
        }

        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("新增")]
        public virtual async Task<AjaxResult> Create(PageCondition[] dtos)
        {
            return null;
        }
    }
}
