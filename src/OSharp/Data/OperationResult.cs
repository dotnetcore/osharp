// -----------------------------------------------------------------------
//  <copyright file="OperationResult.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-08-03 18:31</last-date>
// -----------------------------------------------------------------------

using System.Linq;

using OSharp.Extensions;


namespace OSharp.Data
{
    /// <summary>
    /// 业务操作结果信息类，对操作结果进行封装
    /// </summary>
    public class OperationResult : OperationResult<object>
    {
        static OperationResult()
        {
            Success = new OperationResult(OperationResultType.Success);
            NoChanged = new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 初始化一个<see cref="OperationResult"/>类型的新实例
        /// </summary>
        public OperationResult()
            : this(OperationResultType.NoChanged)
        { }

        /// <summary>
        /// 初始化一个<see cref="OperationResult"/>类型的新实例
        /// </summary>
        public OperationResult(OperationResultType resultType)
            : this(resultType, null, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="OperationResult"/>类型的新实例
        /// </summary>
        public OperationResult(OperationResultType resultType, string message)
            : this(resultType, message, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="OperationResult"/>类型的新实例
        /// </summary>
        public OperationResult(OperationResultType resultType, string message, object data)
            : base(resultType, message, data)
        { }

        /// <summary>
        /// 获取 成功的操作结果
        /// </summary>
        public static OperationResult Success { get; private set; }

        /// <summary>
        /// 获取 未变更的操作结果
        /// </summary>
        public new static OperationResult NoChanged { get; private set; }

        /// <summary>
        /// 将<see cref="OperationResult{TData}"/>转换为<see cref="OperationResult"/>
        /// </summary>
        /// <returns></returns>
        public OperationResult<T> ToOperationResult<T>()
        {
            T data = default(T);
            if (Data is T variable)
            {
                data = variable;
            }
            return new OperationResult<T>(ResultType, Message, data);
        }
    }


    /// <summary>
    /// 泛型版本的业务操作结果信息类，对操作结果进行封装
    /// </summary>
    /// <typeparam name="TData">返回数据的类型</typeparam>
    public class OperationResult<TData> : OsharpResult<OperationResultType, TData>
    {
        static OperationResult()
        {
            NoChanged = new OperationResult<TData>(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 初始化一个<see cref="OperationResult"/>类型的新实例
        /// </summary>
        public OperationResult()
            : this(OperationResultType.NoChanged)
        { }

        /// <summary>
        /// 初始化一个<see cref="OperationResult{TData}"/>类型的新实例
        /// </summary>
        public OperationResult(OperationResultType resultType)
            : this(resultType, null, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="OperationResult{TData}"/>类型的新实例
        /// </summary>
        public OperationResult(OperationResultType resultType, string message)
            : this(resultType, message, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="OperationResult{TData}"/>类型的新实例
        /// </summary>
        public OperationResult(OperationResultType resultType, string message, TData data)
            : base(resultType, message, data)
        { }

        /// <summary>
        /// 获取或设置 返回消息
        /// </summary>
        public override string Message
        {
            get { return _message ?? ResultType.ToDescription(); }
            set { _message = value; }
        }

        /// <summary>
        /// 获取 未变更的操作结果
        /// </summary>
        public static OperationResult<TData> NoChanged { get; private set; }

        /// <summary>
        /// 获取 是否成功
        /// </summary>
        public bool Succeeded => ResultType == OperationResultType.Success;

        /// <summary>
        /// 获取 是否失败
        /// </summary>
        public bool Error
        {
            get
            {
                bool contains = new[] { OperationResultType.ValidError, OperationResultType.QueryNull, OperationResultType.Error }.Contains(ResultType);
                return contains;
            }
        }

        /// <summary>
        /// 将<see cref="OperationResult{TData}"/>转换为<see cref="OperationResult"/>
        /// </summary>
        /// <returns></returns>
        public OperationResult ToOperationResult()
        {
            return new OperationResult(ResultType, Message, Data);
        }

        public OperationResult<TData2> ToOperationResult<TData2>(TData2 data = null) where TData2 : class
        {
            return new OperationResult<TData2>(ResultType, Message, data);
        }
    }
}