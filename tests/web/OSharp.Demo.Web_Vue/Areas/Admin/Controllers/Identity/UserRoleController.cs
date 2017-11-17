// -----------------------------------------------------------------------
//  <copyright file="UserRoleController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-16 15:38</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.UI;
using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Entities;
using OSharp.Entity;
using OSharp.Filter;


namespace OSharp.Demo.Web_Vue.Areas.Admin.Controllers
{
    [Description("管理-用户角色信息")]
    [Area("Admin")]
    [Route("api/[area]/[controller]/[action]")]
    public class UserRoleController : Controller
    {
        private readonly IIdentityContract _identityContract;

        public UserRoleController(IIdentityContract identityContract)
        {
            _identityContract = identityContract;
        }

        [Description("读取")]
        public IActionResult Read()
        {
            PageRequest request = new PageRequest(Request);
            Expression<Func<UserRole, bool>> predicate = FilterHelper.GetExpression<UserRole>(request.FilterGroup);
            var page = _identityContract.UserRoles.ToPage(predicate, request.PageCondition, m => new
            {
                m.Id,
                m.UserId,
                m.RoleId,
                m.IsLocked,
                m.CreatedTime,
                UserName = _identityContract.Users.Where(n=>n.Id == m.UserId).Select(n=>n.UserName).FirstOrDefault(),
                RoleName = _identityContract.Roles.Where(n=>n.Id == m.RoleId).Select(n=>n.Name).FirstOrDefault(),
            });

            return Json(page.ToPageData());
        }
    }
}