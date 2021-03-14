// -----------------------------------------------------------------------
//  <copyright file="SnowKeyGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-14 13:50</last-date>
// -----------------------------------------------------------------------

using OSharp.Data.Snows;


namespace OSharp.Entity.KeyGenerate
{
    /// <summary>
    /// 雪花算法主键生成器
    /// </summary>
    public class SnowKeyGenerator : IKeyGenerator<long>
    {
        private readonly IIdGenerator _idGenerator;

        /// <summary>
        /// 初始化一个<see cref="SnowKeyGenerator"/>类型的新实例
        /// </summary>
        public SnowKeyGenerator(IIdGenerator idGenerator)
        {
            _idGenerator = idGenerator;
        }

        /// <summary>
        /// 获取一个<see cref="long"/>类型的主键数据
        /// </summary>
        /// <returns></returns>
        public long Create()
        {
            return _idGenerator.NewLong();
        }
    }
}