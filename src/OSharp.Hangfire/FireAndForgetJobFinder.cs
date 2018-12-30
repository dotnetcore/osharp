using System;
using System.Linq;
using System.Reflection;

using OSharp.Finders;
using OSharp.Reflection;


namespace OSharp.Hangfire
{
    public class FireAndForgetJobFinder : FinderBase<Type>, IFireAndForgetJobFinder
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="FireAndForgetJobFinder"/>类型的新实例
        /// </summary>
        public FireAndForgetJobFinder(IAllAssemblyFinder allAssemblyFinder)
        {
            _allAssemblyFinder = allAssemblyFinder;
        }

        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Assembly[] assemblies = _allAssemblyFinder.FindAll(true);
            return assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsDeriveClassFrom<IFireAndForgetJob>()).Distinct().ToArray();
        }
    }
}
