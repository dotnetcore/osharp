// -----------------------------------------------------------------------
//  <copyright file="HubClientBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-29 22:02</last-date>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using OSharp.Core.Options;
using OSharp.Data;
using OSharp.Wpf.Data;
using OSharp.Wpf.Hubs.Reflection;
using OSharp.Wpf.Stylet;


namespace OSharp.Wpf.Hubs
{
    /// <summary>
    /// SignalR通信客户端基类
    /// </summary>
    public abstract class HubClientBase : IHubClient
    {

        /// <summary>
        /// 随机对象
        /// </summary>
        protected readonly Random Random = new Random();

        /// <summary>
        /// 获取 网络通信连接
        /// </summary>
        protected HubConnection HubConnection { get; set; }

        /// <summary>
        /// 是否正在连接
        /// </summary>
        protected bool IsConnecting { get; set; }

        /// <summary>
        /// 获取 服务器地址
        /// </summary>
        public virtual string HostUrl => AppSettingsReader.GetString("HostUrl");

        /// <summary>
        /// 获取 客户端版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 获取或设置 当前网络是否通畅
        /// </summary>
        public bool IsNetConnected { get; set; } = true;

        /// <summary>
        /// 获取 当前通信是否连接
        /// </summary>
        public bool IsHubConnected { get; private set; }

        /// <summary>
        /// 网络通信初始化
        /// </summary>
        public virtual void Initialize()
        {
            Check.NotNull(HostUrl, nameof(HostUrl));
            HubConnection = new HubConnectionBuilder().WithUrl($"{HostUrl}/assist-hub",
                opts =>
                {
                    opts.Headers["Version"] = Version;
                    opts.Headers["HostName"] = Dns.GetHostName();
                    opts.AccessTokenProvider = () => Task.FromResult(string.Empty);
                }).Build();
            HubConnection.Closed += OnClose;
            HubListenOn(HubConnection);
        }

        /// <summary>
        /// 开始通信
        /// </summary>
        public virtual async Task<OperationResult> Start()
        {
            try
            {
                Stopwatch watch = Stopwatch.StartNew();
                await HubConnection.StartAsync();
                watch.Stop();
                IsHubConnected = true;
                object[] args = { new[] { WpfConstants.GroupNameWpf } };
                await SendToHub("AddToGroup", args);
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                IsHubConnected = false;
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        /// 停止通信
        /// </summary>
        public virtual async Task Stop()
        {
            object[] args = { new[] { WpfConstants.GroupNameWpf } };
            await SendToHub("RemoveFromGroup", args);
            await HubConnection.StopAsync();
        }

        /// <summary>
        /// 向服务器发送数据
        /// </summary>
        /// <param name="methodName">Hub方法名</param>
        /// <param name="args">调用参数</param>
        /// <returns></returns>
        public async Task SendToHub(string methodName, params object[] args)
        {
            await WaitForConnected();
            await HubConnection.InvokeCoreAsync(methodName, args);
        }

        /// <summary>
        /// 向服务器请求并返回数据
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<TResult> RequestFromHub<TResult>(string methodName, params object[] args)
        {
            await WaitForConnected();
            return await HubConnection.InvokeCoreAsync<TResult>(methodName, args);
        }

        /// <summary>
        /// 在连接关闭时触发
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        protected virtual async Task OnClose(Exception error)
        {
            await Task.Delay(Random.Next(0, 5) * 1000);
            await HubConnection.StartAsync();
        }

        /// <summary>
        /// 重写以实现<see cref="HubConnection"/>通信监听
        /// </summary>
        /// <param name="connection">连接对象</param>
        protected virtual void HubListenOn(HubConnection connection)
        { }

        /// <summary>
        /// 等待通信连接通畅
        /// </summary>
        /// <returns></returns>
        protected virtual async Task WaitForConnected()
        {
            if (IsHubConnected)
            {
                IsConnecting = false;
                return;
            }

            while (IsConnecting)
            {
                await Task.Delay(200);
                if (IsHubConnected)
                {
                    IsConnecting = false;
                    return;
                }

                if (IsConnecting)
                {
                    continue;
                }

                if (IsHubConnected)
                {
                    return;
                }

                break;
            }

            IsConnecting = true;
            while (!IsHubConnected)
            {
                int delay;
                while (!IsNetConnected)
                {
                    delay = Random.Next(6, 12);
                    Output.StatusBar($"当前网络不通畅，延时{delay}秒后重试");
                    await Task.Delay(delay * 1000);
                }

                delay = Random.Next(3, 8);
                Output.StatusBar($"服务器连接失败，延时{delay}秒后重试");
                await Task.Delay(delay * 1000);
                await Start();
            }

            IsConnecting = false;
        }
    }


    /// <summary>
    /// SignalR通信客户端基类
    /// </summary>
    public abstract class HubClientBase<TRequest, TEvent> : HubClientBase, IHubClient<TRequest>
    {
        /// <summary>
        /// 向服务器发送数据
        /// </summary>
        /// <param name="call">调用委托</param>
        /// <returns></returns>
        public async Task SendToHub(Expression<Action<TRequest>> call)
        {
            Invocation invocation = call.GetInvocation();
            await WaitForConnected();
            await HubConnection.InvokeCoreAsync(invocation.MethodName, invocation.Parameters);
        }

        /// <summary>
        /// 向服务器请求并返回数据
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="call">调用委托</param>
        /// <returns></returns>
        public async Task<TResult> RequestFromHub<TResult>(Expression<Func<TRequest, Task<TResult>>> call)
        {
            Invocation invocation = call.GetInvocation();
            await WaitForConnected();
            return await HubConnection.InvokeCoreAsync<TResult>(invocation.MethodName, invocation.Parameters);
        }

        /// <summary>
        /// 重写以实现<see cref="HubClientBase.HubConnection"/>通信监听
        /// </summary>
        /// <param name="connection">连接对象</param>
        protected override void HubListenOn(HubConnection connection)
        {
            HubConnection.On<TEvent>(IoC.Get<TEvent>());
            base.HubListenOn(connection);
        }
    }

}