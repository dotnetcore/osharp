// -----------------------------------------------------------------------
//  <copyright file="UserRoleController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-10 16:57</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.UI;
using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Entity;
using OSharp.Filter;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [Description("管理-用户角色信息")]
    public class UserRoleController : AdminController
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
                UserName = _identityContract.Users.Where(n => n.Id == m.UserId).Select(n => n.UserName).FirstOrDefault(),
                RoleName = _identityContract.Roles.Where(n => n.Id == m.RoleId).Select(n => n.Name).FirstOrDefault(),
            });

            return Json(page.ToPageData());
        }

        [HttpPost]
        [Description("新增")]
        public async Task<IActionResult> Create(UserRoleInputDto[] dtos)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Description("更新")]
        public async Task<IActionResult> Update(UserRoleInputDto[] dtos)
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