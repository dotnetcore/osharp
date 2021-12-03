// -----------------------------------------------------------------------
//  <copyright file="SystemsService.MenuInfo.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-02-28 23:54</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;
using OSharp.Exceptions;
using OSharp.Hosting.Systems.Dtos;
using OSharp.Hosting.Systems.Entities;


namespace OSharp.Hosting.Systems
{
    public partial class SystemsService
    {
        /// <summary>
        /// 获取 菜单信息查询数据集
        /// </summary>
        public IQueryable<Menu> MenuInfos => MenuInfoRepository.QueryAsNoTracking();

        /// <summary>
        /// 检查菜单信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的菜单信息编号</param>
        /// <returns>菜单信息是否存在</returns>
        public Task<bool> CheckMenuInfoExists(Expression<Func<Menu, bool>> predicate, int id = default)
        {
            return MenuInfoRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 添加菜单信息信息
        /// </summary>
        /// <param name="dtos">要添加的菜单信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> CreateMenuInfos(params MenuInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            Check.Validate<MenuInputDto, int>(dtos, nameof(dtos));

            return MenuInfoRepository.InsertAsync(dtos,
                async dto =>
                {
                    if (await MenuInfoRepository.CheckExistsAsync(m => m.Name == dto.Name))
                    {
                        throw new OsharpException($"名称为“{dto.Name}”的菜单信息已存在，请更换");
                    }
                },
                async (dto, entity) =>
                {
                    if (dto.ParentId != null)
                    {
                        Menu parent = await MenuInfoRepository.GetAsync(dto.ParentId.Value);
                        if (parent == null)
                        {
                            throw new OsharpException($"编号为“{dto.ParentId}”的菜单信息不存在");
                        }

                        if (await MenuInfoRepository.CheckExistsAsync(m => m.ParentId == dto.ParentId && m.Text == dto.Text))
                        {
                            throw new OsharpException($"菜单“{parent.Text}”的子菜单中已存在显示名称为“{dto.Text}”的菜单项，请更换");
                        }
                    }

                    return entity;
                });
        }

        /// <summary>
        /// 更新菜单信息信息
        /// </summary>
        /// <param name="dtos">包含更新信息的菜单信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> UpdateMenuInfos(params MenuInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            Check.Validate<MenuInputDto, int>(dtos, nameof(dtos));

            return MenuInfoRepository.UpdateAsync(dtos,
                async (dto, entity) =>
                {
                    if (await MenuInfoRepository.CheckExistsAsync(m => m.Name == dto.Name, entity.Id))
                    {
                        throw new OsharpException($"名称为“{dto.Name}”的菜单信息已存在，请更换");
                    }

                    if (entity.IsSystem)
                    {
                        throw new OsharpException($"菜单“{dto.Name}”是系统菜单，不能更新");
                    }
                    if (dto.ParentId != entity.ParentId && dto.ParentId != null)
                    {
                        Menu parent = await MenuInfoRepository.GetAsync(dto.ParentId.Value);
                        if (parent == null)
                        {
                            throw new OsharpException($"编号为“{dto.ParentId}”的菜单信息不存在");
                        }

                        if (await MenuInfoRepository.CheckExistsAsync(m => m.ParentId == dto.ParentId && m.Text == dto.Text, entity.Id))
                        {
                            throw new OsharpException($"菜单“{parent.Text}”的子菜单中已存在显示名称为“{dto.Text}”的菜单项，请更换");
                        }
                    }
                });
        }

        /// <summary>
        /// 删除菜单信息信息
        /// </summary>
        /// <param name="ids">要删除的菜单信息编号</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> DeleteMenuInfos(params int[] ids)
        {
            Check.NotNull(ids, nameof(ids));
            return MenuInfoRepository.DeleteAsync(ids,
                async entity =>
                {
                    if (await MenuInfoRepository.CheckExistsAsync(m => m.ParentId == entity.Id))
                    {
                        throw new OsharpException($"菜单“{entity.Text}”的子菜单不为空，不能删除");
                    }

                    if (entity.IsSystem)
                    {
                        throw new OsharpException($"菜单“{entity.Name}”是系统菜单，不能删除");
                    }
                });
        }
    }
}