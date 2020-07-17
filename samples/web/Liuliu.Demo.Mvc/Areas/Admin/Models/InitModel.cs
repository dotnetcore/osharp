// -----------------------------------------------------------------------
//  <copyright file="InitModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-06-14 19:15</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;


namespace Liuliu.Demo.Web.Areas.Admin.Models
{
    public class InitModel
    {
        public InitModel()
        {
            MenuInfo = new List<MenuInfo>();
        }

        public HomeInfo HomeInfo { get; set; }

        public LogoInfo LogoInfo { get; set; }

        public List<MenuInfo> MenuInfo { get; set; }
    }


    public class HomeInfo
    {
        public string Title { get; set; }

        public string Icon { get; set; }

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
