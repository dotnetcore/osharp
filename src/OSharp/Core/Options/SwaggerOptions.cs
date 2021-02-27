// -----------------------------------------------------------------------
//  <copyright file="SwaggerOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-12-14 23:28</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;


namespace OSharp.Core.Options
{
    /// <summary>
    /// Swagger选项
    /// </summary>
    public class SwaggerOptions
    {
        public ICollection<SwaggerEndpoint> Endpoints { get; set; } = new List<SwaggerEndpoint>();

        public string RoutePrefix { get; set; }

        public bool IsHideSchemas { get; set; } = false;

        public bool MiniProfiler { get; set; } = true;

        public bool Enabled { get; set; }
    }


    public class SwaggerEndpoint
    {
        public string Title { get; set; }

        public string Version { get; set; }

        public string Url { get; set; }
    }
}