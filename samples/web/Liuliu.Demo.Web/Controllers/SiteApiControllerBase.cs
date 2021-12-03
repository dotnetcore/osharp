// -----------------------------------------------------------------------
//  <copyright file="SiteApiController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-07 1:12</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Authorization;

using OSharp.AspNetCore.Mvc;
using OSharp.Authorization;


namespace Liuliu.Demo.Web.Controllers
{
    [DisplayName("网站")]
    [ApiAuthorize]
    public abstract class SiteApiControllerBase : ApiControllerBase
    { }
}
