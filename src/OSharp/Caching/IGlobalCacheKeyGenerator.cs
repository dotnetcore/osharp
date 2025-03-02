using System.Threading.Tasks;

namespace OSharp.Caching
{
    /// <summary>
    /// 全局缓存键生成器接口
    /// </summary>
    public interface IGlobalCacheKeyGenerator
    {
        /// <summary>
        /// 获取全局缓存键
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>缓存键</returns>
        string GetGlobalKey(params object[] args);

        /// <summary>
        /// 异步获取全局缓存键
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>缓存键</returns>
        Task<string> GetGlobalKeyAsync(params object[] args);
    }
}
