using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Dependency
{
    /// <summary>
    /// 默认<see cref="IServiceScope"/>工厂，行为和<see cref="IServiceScopeFactory"/>一样
    /// </summary>
    public class DefaultServiceScopeFactory : IHybridServiceScopeFactory
    {
        /// <summary>
        /// 初始化一个<see cref="DefaultServiceScopeFactory"/>类型的新实例
        /// </summary>
        public DefaultServiceScopeFactory(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }
        
        /// <summary>
        /// 获取 <see cref="IServiceScope"/>工厂
        /// </summary>
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        #region Implementation of IServiceScopeFactory

        /// <summary>
        /// Create an <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" /> which
        /// contains an <see cref="T:System.IServiceProvider" /> used to resolve dependencies from a
        /// newly created scope.
        /// </summary>
        /// <returns>
        /// An <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" /> controlling the
        /// lifetime of the scope. Once this is disposed, any scoped services that have been resolved
        /// from the <see cref="P:Microsoft.Extensions.DependencyInjection.IServiceScope.ServiceProvider" />
        /// will also be disposed.
        /// </returns>
        public IServiceScope CreateScope()
        {
            return ServiceScopeFactory.CreateScope();
        }

        #endregion
    }
}
