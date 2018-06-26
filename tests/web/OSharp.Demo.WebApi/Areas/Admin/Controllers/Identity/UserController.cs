// -----------------------------------------------------------------------
//  <copyright file="UserController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-14 0:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Collections;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Security;
using OSharp.Demo.Security.Dtos;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Filter;
using OSharp.Identity;
using OSharp.Mapping;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 1, Position = "Identity")]
    [Description("管理-用户信息")]
    public class UserController : AdminApiController
    {
        private readonly IIdentityContract _identityContract;
        private readonly SecurityManager _securityManager;
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager, SecurityManager securityManager, IIdentityContract identityContract)
        {
            _userManager = userManager;
            _securityManager = securityManager;
            _identityContract = identityContract;
        }

        /// <summary>
        /// 读取用户列表信息
        /// </summary>
        /// <returns>用户列表信息</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public PageData<UserOutputDto> Read()
        {
            PageRequest request = new PageRequest(Request);
            Expression<Func<User, bool>> predicate = FilterHelper.GetExpression<User>(request.FilterGroup);
            var page = _userManager.Users.ToPage<User, UserOutputDto>(predicate, request.PageCondition);

            return page.ToPageData();
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="dtos">用户信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("新增")]
        public async Task<AjaxResult> Create(UserInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            List<string> names = new List<string>();
            foreach (var dto in dtos)
            {
                User user = dto.MapTo<User>();
                IdentityResult result = dto.Password.IsMissing()
                    ? await _userManager.CreateAsync(user)
                    : await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult().ToAjaxResult();
                }
                names.Add(user.UserName);
            }
            return new AjaxResult($"用户“{names.ExpandAndToString()}”创建成功");
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="dtos">用户信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新")]
        public async Task<AjaxResult> Update(UserInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            List<string> names = new List<string>();
            foreach (var dto in dtos)
            {
                User user = await _userManager.FindByIdAsync(dto.Id.ToString());
                user = dto.MapTo(user);
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult().ToAjaxResult();
                }
                names.Add(user.UserName);
            }
            return new AjaxResult($"用户“{names.ExpandAndToString()}”更新成功");
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="ids">用户信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除")]
        public async Task<AjaxResult> Delete(int[] ids)
        {
            Check.NotNull(ids, nameof(ids));
            List<string> names = new List<string>();
            foreach (int id in ids)
            {
                User user = await _userManager.FindByIdAsync(id.ToString());
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult().ToAjaxResult();
                }
                names.Add(user.UserName);
            }
            return new AjaxResult($"用户“{names.ExpandAndToString()}”删除成功");
        }

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="dto">用户权限信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [DependOnFunction("ReadUserRoles", Controller = "Role")]
        [DependOnFunction("ReadUserModules", Controller = "Module")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("设置权限")]
        public async Task<AjaxResult> SetPermission([FromBody] UserSetPermissionDto dto)
        {
            OperationResult result1 = await _identityContract.SetUserRoles(dto.UserId, dto.RoleIds);
            string msg = $"设置角色：{result1.Message}<br/>";
            OperationResult result2 = await _securityManager.SetUserModules(dto.UserId, dto.ModuleIds);
            msg += $"模块设置：{result2.Message}";

            AjaxResultType type;
            if (result1.ResultType == OperationResultType.NoChanged && result2.ResultType == OperationResultType.NoChanged)
            {
                type = AjaxResultType.Info;
            }
            else if (new[] { result1.ResultType, result2.ResultType }.Contains(OperationResultType.Success))
            {
                type = AjaxResultType.Success;
            }
            else
            {
                type = AjaxResultType.Error;
            }

            return new AjaxResult(msg, type);
        }
    }
}