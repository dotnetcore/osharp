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
        private string _href;

        public string Title { get; set; }

        public string Icon { get; set; }

        public string Href
        {
            get => _href;
            set
            {
                // Url.Action生成的URL以/开头，会造成前端hash时显示#//admin/xxx，要去掉
                if (value.StartsWith('/'))
                {
                    value = value.TrimStart('/');
                }
                _href = value;
            }
        }

        public string Target { get; set; }

        public List<MenuInfo> Child { get; set; } = new List<MenuInfo>();
    }
}
