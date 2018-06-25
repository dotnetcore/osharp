// -----------------------------------------------------------------------
//  <copyright file="SystemManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-25 21:00</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;

using OSharp.Caching;
using OSharp.Data;
using OSharp.Entity;


namespace OSharp.System
{
    /// <summary>
    /// 系统管理器
    /// </summary>
    public class SystemManager : IKeyValueCoupleStore
    {
        private readonly IRepository<KeyValueCouple, Guid> _keyValueRepository;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 初始化一个<see cref="SystemManager"/>类型的新实例
        /// </summary>
        public SystemManager(IRepository<KeyValueCouple, Guid> keyValueRepository,
            IDistributedCache cache)
        {
            _keyValueRepository = keyValueRepository;
            _cache = cache;
        }

        #region Implementation of IKeyValueCoupleStore

        private const string AllKeyValueCouplesKey = "All_KeyValueCouple_Key";

        /// <summary>
        /// 获取 键值对数据查询数据集
        /// </summary>
        public IQueryable<KeyValueCouple> KeyValueCouples
        {
            get { return _keyValueRepository.Entities; }
        }

        /// <summary>
        /// 获取指定键名的数据项
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>数据项</returns>
        public KeyValueCouple GetKeyValueCouple(string key)
        {
            const int seconds = 60 * 1000;
            KeyValueCouple[] pairs = _cache.Get(AllKeyValueCouplesKey, () => _keyValueRepository.Entities.ToArray(), seconds);
            return pairs.FirstOrDefault(m => m.Key == key);
        }

        /// <summary>
        /// 检查键值对信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的键值对信息编号</param>
        /// <returns>键值对信息是否存在</returns>
        public Task<bool> CheckKeyValueCoupleExists(Expression<Func<KeyValueCouple, bool>> predicate, Guid id = default(Guid))
        {
            return _keyValueRepository.CheckExistsAsync(predicate, id);
        }

        /// <summary>
        /// 添加或更新键值对信息信息
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>业务操作结果</returns>
        public Task<OperationResult> CreateOrUpdateKeyValueCouple(string key, object value)
        {
            KeyValueCouple pair = new KeyValueCouple(key, value);
            return CreateOrUpdateKeyValueCouples(pair);
        }

        /// <summary>
        /// 添加或更新键值对信息信息
        /// </summary>
        /// <param name="dtos">要添加的键值对信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> CreateOrUpdateKeyValueCouples(params KeyValueCouple[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            foreach (KeyValueCouple dto in dtos)
            {
                KeyValueCouple pair = _keyValueRepository.TrackEntities.FirstOrDefault(m => m.Key == dto.Key);
                if (pair == null)
                {
                    pair = dto;
                    await _keyValueRepository.InsertAsync(pair);
                }
                else
                {
                    pair.Value = dto.Value;
                    await _keyValueRepository.UpdateAsync(pair);
                }
            }
            await _cache.RemoveAsync(AllKeyValueCouplesKey);
            return OperationResult.Success;
        }

        /// <summary>
        /// 删除键值对信息信息
        /// </summary>
        /// <param name="ids">要删除的键值对信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteKeyValueCouples(params Guid[] ids)
        {
            OperationResult result = await _keyValueRepository.DeleteAsync(ids);
            if (result.Successed)
            {
                await _cache.RemoveAsync(AllKeyValueCouplesKey);
            }
            return result;
        }

        /// <summary>
        /// 删除以根键路径为起始的所有字典项，如输入“System.User.”，所有键以此开头的项都会被删除
        /// </summary>
        /// <param name="rootKey">根键路径</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteKeyValueCouples(string rootKey)
        {
            Guid[] ids = _keyValueRepository.Entities.Where(m => m.Key.StartsWith(rootKey)).Select(m => m.Id).ToArray();
            return await DeleteKeyValueCouples(ids);
        }

        #endregion
    }
}