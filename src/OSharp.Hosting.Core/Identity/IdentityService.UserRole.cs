// -----------------------------------------------------------------------
//  <copyright file="IdentityService.UserRole.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;

using Microsoft.AspNetCore.Identity;

using OSharp.Collections;
using OSharp.Data;
using OSharp.Identity;
using OSharp.Identity.Events;


namespace OSharp.Hosting.Identity
{
    public partial class IdentityService
    {
        /// <summary>
        /// 获取 用户角色信息查询数据集
        /// </summary>
        public IQueryable<UserRole> UserRoles
        {
            get { return UserRoleRepository.QueryAsNoTracking(); }
        }

        /// <summary>
        /// 检查用户角色信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的用户角色信息编号</param>
        /// <returns>用户角色信息是否存在</returns>
        public Task<bool> CheckUserRoleExists(Expression<Func<UserRole, bool>> predicate, Guid id = default(Guid))
        {
            return UserRoleRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 更新用户角色信息
        /// </summary>
        /// <param name="dtos">用户角色信息集合</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> UpdateUserRoles(params UserRoleInputDto[] dtos)
        {
            Check.Validate<UserRoleInputDto,Guid>(dtos, nameof(dtos));

            List<string> userNames = new List<string>();
            OperationResult result = await UserRoleRepository.UpdateAsync(dtos,
                (dto, entity) =>
                {
                    string userName = UserRoleRepository.QueryAsNoTracking(m => m.UserId == entity.UserId).Select(m => m.User.UserName).FirstOrDefault();
                    userNames.AddIfNotNull(userName);
                    return Task.FromResult(0);
                });
            if (result.Succeeded && userNames.Count > 0)
            {
                OnlineUserCacheRemoveEventData eventData = new OnlineUserCacheRemoveEventData() { UserNames = userNames.ToArray() };
                await EventBus.PublishAsync(eventData);
            }
            return result;
        }

        /// <summary>
        /// 删除用户角色信息
        /// </summary>
        /// <param name="ids">用户角色信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteUserRoles(Guid[] ids)
        {
            List<string>userNames = new List<string>();
            OperationResult result = await UserRoleRepository.DeleteAsync(ids,
                (entity) =>
                {
                    string userName = UserRoleRepository.QueryAsNoTracking(m => m.UserId == entity.UserId).Select(m => m.User.UserName).FirstOrDefault();
                    userNames.AddIfNotNull(userName);
                    return Task.FromResult(0);
                });
            if (result.Succeeded && userNames.Count > 0)
            {
                OnlineUserCacheRemoveEventData eventData = new OnlineUserCacheRemoveEventData(){UserNames = userNames.ToArray()};
                await EventBus.PublishAsync(eventData);
            }

            return result;
        }

        /// <summary>
        /// 设置用户的角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleIds">角色编号集合</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> SetUserRoles(int userId, int[] roleIds)
        {
            User user = await UserManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new OperationResult(OperationResultType.QueryNull, $"编号为“{userId}”的用户不存在");
            }
            IList<string> roleNames = RoleManager.Roles.Where(m => roleIds.Contains(m.Id)).Select(m => m.Name).ToList();
            IList<string> existRoleNames = await UserManager.GetRolesAsync(user);
            string[] addRoleNames = roleNames.Except(existRoleNames).ToArray();
            string[] removeRoleNames = existRoleNames.Except(roleNames).ToArray();

            if (!addRoleNames.Union(removeRoleNames).Any())
            {
                return OperationResult.NoChanged;
            }

            try
            {
                IdentityResult result = await UserManager.AddToRolesAsync(user, addRoleNames);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult();
                }
                result = await UserManager.RemoveFromRolesAsync(user, removeRoleNames);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult();
                }
                await UserManager.UpdateSecurityStampAsync(user);

                //更新用户缓存使角色生效
                OnlineUserCacheRemoveEventData eventData = new OnlineUserCacheRemoveEventData() { UserNames = new[] { user.UserName } };
                await EventBus.PublishAsync(eventData);
            }
            catch (InvalidOperationException ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
            if (addRoleNames.Length > 0 && removeRoleNames.Length == 0)
            {
                return new OperationResult(OperationResultType.Success, $"用户“{user.UserName}”添加角色“{addRoleNames.ExpandAndToString()}”操作成功");
            }
            if (addRoleNames.Length == 0 && removeRoleNames.Length > 0)
            {
                return new OperationResult(OperationResultType.Success, $"用户“{user.UserName}”移除角色“{removeRoleNames.ExpandAndToString()}”操作成功");
            }
            return new OperationResult(OperationResultType.Success,
                $"用户“{user.UserName}”添加角色“{addRoleNames.ExpandAndToString()}”，移除角色“{removeRoleNames.ExpandAndToString()}”操作成功");
        }
    }
}