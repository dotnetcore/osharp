// -----------------------------------------------------------------------
//  <copyright file="JsonContent.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-11-08 16:45</last-date>
// -----------------------------------------------------------------------

using System.Net.Http;
using System.Text;

using Newtonsoft.Json;


namespace OSharp.Http
{
    /// <summary>
    /// Json的HttpContent
    /// </summary>
    public class JsonContent : StringContent
    {
        /// <summary>
        /// 初始化一个<see cref="JsonContent"/>类型的新实例
        /// </summary>
        public JsonContent(object obj)
            : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        { }
    }
}