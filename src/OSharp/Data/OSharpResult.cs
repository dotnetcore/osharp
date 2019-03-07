// -----------------------------------------------------------------------
//  <copyright file="OSharpResult.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-08-03 18:29</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Extensions;


namespace OSharp.Data
{
    /// <summary>
    /// OSharp结果基类
    /// </summary>
    /// <typeparam name="TResultType"></typeparam>
    public abstract class OsharpResult<TResultType> : OsharpResult<TResultType, object>, IOsharpResult<TResultType>
    {
        /// <summary>
        /// 初始化一个<see cref="OsharpResult{TResultType}"/>类型的新实例
        /// </summary>
        protected OsharpResult()
            : this(default(TResultType))
        { }

        /// <summary>
        /// 初始化一个<see cref="OsharpResult{TResultType}"/>类型的新实例
        /// </summary>
        protected OsharpResult(TResultType type)
            : this(type, null, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="OsharpResult{TResultType}"/>类型的新实例
        /// </summary>
        protected OsharpResult(TResultType type, string message)
            : this(type, message, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="OsharpResult{TResultType}"/>类型的新实例
        /// </summary>
        protected OsharpResult(TResultType type, string message, object data)
            : base(type, message, data)
        { }
    }


    /// <summary>
    /// OSharp结果基类
    /// </summary>
    /// <typeparam name="TResultType">结果类型</typeparam>
    /// <typeparam name="TData">结果数据类型</typeparam>
    public abstract class OsharpResult<TResultType, TData> : IOsharpResult<TResultType, TData>
    {
        /// <summary>
        /// 内部消息
        /// </summary>
        protected string _message;

        /// <summary>
        /// 初始化一个<see cref="OsharpResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected OsharpResult()
            : this(default(TResultType))
        { }

        /// <summary>
        /// 初始化一个<see cref="OsharpResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected OsharpResult(TResultType type)
            : this(type, null, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="OsharpResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected OsharpResult(TResultType type, string message)
            : this(type, message, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="OsharpResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected OsharpResult(TResultType type, string message, TData data)
        {
            if (message == null && typeof(TResultType).IsEnum)
            {
                message = (type as Enum)?.ToDescription();
            }
            ResultType = type;
            _message = message;
            Data = data;
        }

        /// <summary>
        /// 获取或设置 结果类型
        /// </summary>
        public TResultType ResultType { get; set; }

        /// <summary>
        /// 获取或设置 返回消息
        /// </summary>
        public virtual string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 获取或设置 结果数据
        /// </summary>
        public TData Data { get; set; }
    }
}