using System.Collections.Generic;


namespace Liuliu.Demo.Web.Areas.Admin.Models
{
    public class InitModel
    {
        public HomeInfo HomeInfo { get; set; }

        public LogoInfo LogoInfo { get; set; }

        public MenuInfo MenuInfo { get; set; }
    }


    public class HomeInfo
    {
        public string Title { get; set; }

        public string Href { get; set; }
    }


    public class LogoInfo
    {
        public string Title { get; set; }

        public string Image { get; set; }

        public string Href { get; set; }
    }


    public class MenuInfo
    {
        public string Title { get; set; }

        public string Icon { get; set; }

        public string Href { get; set; }

        public string Target { get; set; }

        public List<MenuInfo> Child { get; set; } = new List<MenuInfo>();
    }
}
