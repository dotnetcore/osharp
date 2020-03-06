// -----------------------------------------------------------------------
//  <copyright file="RoleStoreBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-01-31 19:14</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using OSharp.Data;
using OSharp.Entity;
using OSharp.Identity.Entities;


namespace OSharp.Identity
{
    /// <summary>
    /// 角色存储基类
    /// </summary>
    /// <typeparam name="TRole">角色实体类型</typeparam>
    /// <typeparam name="TRoleKey">角色编号类型</typeparam>
    /// <typeparam name="TRoleClaim">角色声明类型</typeparam>
    /// <typeparam name="TRoleClaimKey">角色声明编号类型</typeparam>
    public abstract class RoleStoreBase<TRole, TRoleKey, TRoleClaim, TRoleClaimKey>
        : Disposable, IQueryableRoleStore<TRole>,
          IRoleClaimStore<TRole>
        where TRole : RoleBase<TRoleKey>
        where TRoleClaim : RoleClaimBase<TRoleClaimKey, TRoleKey>, new()
        where TRoleKey : IEquatable<TRoleKey>
        where TRoleClaimKey : IEquatable<TRoleClaimKey>
    {
        private readonly IRepository<TRoleClaim, TRoleClaimKey> _roleClaimRepository;
        private readonly IRepository<TRole, TRoleKey> _roleRepository;

        /// <summary>
        /// 初始化一个<see cref="RoleStoreBase{TRole,TRoleKey,TRoleClaim, TRoleClaimKey}"/>类型的新实例
        /// </summary>
        /// <param name="roleRepository">角色仓储</param>
        /// <param name="roleClaimRepository">角色声明仓储</param>
        protected RoleStoreBase(
            IRepository<TRole, TRoleKey> roleRepository,
            IRepository<TRoleClaim, TRoleClaimKey> roleClaimRepository)
        {
            _roleRepository = roleRepository;
            _roleClaimRepository = roleClaimRepository;
        }

        #region Implementation of IQueryableRoleStore<TRole>

        /// <summary>
        /// Returns an <see cref="T:System.Linq.IQueryable`1" /> collection of roles.
        /// </summary>
        /// <value>An <see cref="T:System.Linq.IQueryable`1" /> collection of roles.</value>
        public IQueryable<TRole> Roles => _roleRepository.QueryAsNoTracking();

        #endregion

        /// <summary>
        /// Converts the provided <paramref name="id"/> to a strongly typed key object.
        /// </summary>
        /// <param name="id">The id to convert.</param>
        /// <returns>An instance of <typeparamref name="TRoleKey"/> representing the provided <paramref name="id"/>.</returns>
        public virtual TRoleKey ConvertIdFromString(string id)
        {
            if (id == null)
            {
                return default(TRoleKey);
            }

            return (TRoleKey)TypeDescriptor.GetConverter(typeof(TRoleKey)).ConvertFromInvariantString(id);
        }

        /// <summary>
        /// Converts the provided <paramref name="id"/> to its string representation.
        /// </summary>
        /// <param name="id">The id to convert.</param>
        /// <returns>An <see cref="string"/> representation of the provided <paramref name="id"/>.</returns>
        public virtual string ConvertIdToString(TRoleKey id)
        {
            if (id.Equals(default(TRoleKey)))
            {
                return null;
            }

            return id.ToString();
        }

        /// <summary>
        /// 如果已释放，则抛出异常
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #region Implementation of IRoleStore<TRole>

        /// <summary>
        /// Creates a new role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to create in the store.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the asynchronous query.</returns>
        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            if (role.IsDefault)
            {
                string defaultRole = _roleRepository.Query(m => m.IsDefault, false).Select(m => m.Name).FirstOrDefault();
                if (defaultRole != null)
                {
                    return new IdentityResult().Failed($"系统中已存在默认角色“{defaultRole}”，不能重复添加");
                }
            }

            await _roleRepository.InsertAsync(role);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Updates a role in a store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the asynchronous query.</returns>
        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            if (role.IsSystem)
            {
                return new IdentityResult().Failed($"角色“{role.Name}”是系统角色，不能更新");
            }

            if (role.IsDefault)
            {
                var defaultRole = _roleRepository.Query(m => m.IsDefault, false).Select(m => new { m.Id, m.Name }).FirstOrDefault();
                if (defaultRole != null && !defaultRole.Id.Equals(role.Id))
                {
                    return new IdentityResult().Failed($"系统中已存在默认角色“{defaultRole.Name}”，不能重复添加");
                }
            }

            await _roleRepository.UpdateAsync(role);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Deletes a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to delete from the store.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the asynchronous query.</returns>
        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            if (role.IsSystem)
            {
                return new IdentityResult().Failed($"角色“{role.Name}”是系统角色，不能删除");
            }

            await _roleRepository.DeleteAsync(role);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Gets the ID for a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that contains the ID of the role.</returns>
        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            return Task.FromResult(ConvertIdToString(role.Id));
        }

        /// <summary>
        /// Gets the name of a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be returned.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that contains the name of the role.</returns>
        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            return Task.FromResult(role.Name);
        }

        /// <summary>
        /// Sets the name of a role in the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            role.Name = roleName;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Get a role's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose normalized name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that contains the name of the role.</returns>
        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        /// <summary>
        /// Set a role's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose normalized name should be set.</param>
        /// <param name="normalizedName">The normalized name to set</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Finds the role who has the specified ID as an asynchronous operation.
        /// </summary>
        /// <param name="roleId">The role ID to look for.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that result of the look up.</returns>
        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            TRoleKey id = ConvertIdFromString(roleId);
            return Task.FromResult(_roleRepository.Query().FirstOrDefault(m => m.Id.Equals(id)));
        }

        /// <summary>
        /// Finds the role who has the specified normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="normalizedRoleName">The normalized role name to look for.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that result of the look up.</returns>
        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Task.FromResult(_roleRepository.Query().FirstOrDefault(m => m.NormalizedName == normalizedRoleName));
        }

        #endregion

        #region Implementation of IRoleClaimStore<TRole>

        /// <summary>
        ///  Gets a list of <see cref="T:System.Security.Claims.Claim" />s to be belonging to the specified <paramref name="role" /> as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose claims to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the result of the asynchronous query, a list of <see cref="T:System.Security.Claims.Claim" />s.
        /// </returns>
        public Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));

            IList<Claim> list = _roleClaimRepository.QueryAsNoTracking(m => m.RoleId.Equals(role.Id))
                .Select(n => new Claim(n.ClaimType, n.ClaimValue)).ToList();
            return Task.FromResult(list);
        }

        /// <summary>
        /// Add a new claim to a role as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to add a claim to.</param>
        /// <param name="claim">The <see cref="T:System.Security.Claims.Claim" /> to add.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));
            Check.NotNull(claim, nameof(claim));

            TRoleClaim roleClaim = new TRoleClaim() { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
            await _roleClaimRepository.InsertAsync(roleClaim);
        }

        /// <summary>
        /// Remove a claim from a role as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The <see cref="T:System.Security.Claims.Claim" /> to remove.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Check.NotNull(role, nameof(role));
            Check.NotNull(claim, nameof(claim));

            return _roleClaimRepository.DeleteBatchAsync(m => m.RoleId.Equals(role.Id) && m.ClaimValue == claim.Type && m.ClaimValue == claim.Value);
        }

        #endregion
    }
}