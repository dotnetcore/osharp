using System;


namespace OSharp.Dependency
{
    /// <summary>
    /// 标注了此特性的类，将忽略依赖注入自动映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface)]
    public class IgnoreDependencyAttribute : Attribute
    {
    }
}
