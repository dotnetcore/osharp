// -----------------------------------------------------------------------
//  <copyright file="IHubClient.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-29 21:59</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using OSharp.Data;


namespace OSharp.Wpf.Hubs
{
    /// <summary>
    /// 定义SignalR通信客户端
    /// </summary>
    public interface IHubClient
    {
        /// <summary>
        /// 获取 通信Hub地址
        /// </summary>
        string HubUrl { get; }

        /// <summary>
        /// 获取或设置 客户端版本
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// 获取 当前网络是否通畅
        /// </summary>
        bool IsNetConnected { get; }

        /// <summary>
        /// 获取 当前通信是否已连接
        /// </summary>
        bool IsHubConnected { get; }

        /// <summary>
        /// 网络通信初始化
        /// </summary>
        void Initialize();

        /// <summary>
        /// 开始通信
        /// </summary>
        /// <returns></returns>
        Task<OperationResult> Start();

        /// <summary>
        /// 停止通信
        /// </summary>
        /// <returns></returns>
        Task Stop();

        /// <summary>
        /// 重启通信
        /// </summary>
        /// <returns></returns>
        Task<OperationResult> Restart();

        /// <summary>
        /// 向服务器发送数据
        /// </summary>
        /// <param name="methodName">Hub方法名</param>
        /// <param name="args">调用参数</param>
        /// <returns></returns>
        Task SendToHub(string methodName, params object[] args);

        /// <summary>
        /// 向服务器请求并返回数据
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<TResult> RequestFromHub<TResult>(string methodName, params object[] args);
    }


    /// <summary>
    /// 定义SignalR通信客户端
    /// </summary>
    /// <typeparam name="TRequest">表示客户端向服务端发起的请求接口契约</typeparam>
    public interface IHubClient<TRequest> : IHubClient
    {
        /// <summary>
        /// 向服务器发送数据
        /// </summary>
        /// <param name="call">调用委托</param>
        /// <returns></returns>
        Task SendToHub(Expression<Action<TRequest>> call);

        /// <summary>
        /// 向服务器请求并返回数据
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="call">调用委托</param>
        /// <returns></returns>
        Task<TResult> RequestFromHub<TResult>(Expression<Func<TRequest, Task<TResult>>> call);
    }
}