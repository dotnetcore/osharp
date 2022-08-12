using System.ComponentModel;

using OSharp.Hosting.Apis.Controllers;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-主页")]
    public class HomeController : SiteApiControllerBase
    {
        /// <summary>
        /// 初始化一个<see cref="SiteApiControllerBase"/>类型的新实例
        /// </summary>
        public HomeController(IServiceProvider provider)
            : base(provider)
        { }
        
    }
}
