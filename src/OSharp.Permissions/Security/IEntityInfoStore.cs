using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;
using OSharp.Dependency;
using OSharp.Core.EntityInfos;


namespace OSharp.Security
{
    /// <summary>
    /// 定义实体信息存储
    /// </summary>
    [IgnoreDependency]
    public interface IEntityInfoStore<TEntityInfo, in TEntityInfoInputDto>
        where TEntityInfo : IEntityInfo
        where TEntityInfoInputDto : EntityInfoInputDtoBase
    {
        #region 实体信息业务

        /// <summary>
        /// 获取 实体信息查询数据集
        /// </summary>
        IQueryable<TEntityInfo> EntityInfos { get; }

        /// <summary>
        /// 检查实体信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的实体信息编号</param>
        /// <returns>实体信息是否存在</returns>
        Task<bool> CheckEntityInfoExists(Expression<Func<TEntityInfo, bool>> predicate, Guid id = default(Guid));

        /// <summary>
        /// 更新实体信息
        /// </summary>
        /// <param name="dtos">包含更新信息的实体信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateEntityInfos(params TEntityInfoInputDto[] dtos);

        #endregion

    }
}
