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
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;

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
        /// 获取 通信Hub地址
        /// </summary>
        public abstract string HubUrl { get; }

        /// <summary>
        /// 获取或设置 客户端版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 获取或设置 当前网络是否通畅
        /// </summary>
        public bool IsNetConnected { get; set; } = true;

        /// <summary>
        /// 获取 当前通信是否已连接
        /// </summary>
        public bool IsHubConnected => HubConnection?.State == HubConnectionState.Connected;

        /// <summary>
        /// 获取 通信是否正在连接
        /// </summary>
        protected bool IsHubConnecting => new[] { HubConnectionState.Reconnecting, HubConnectionState.Reconnecting }.Contains(HubConnection.State);

        /// <summary>
        /// 网络通信初始化
        /// </summary>
        public virtual void Initialize()
        {
            Check.NotNull(HubUrl, nameof(HubUrl));
            HubConnection = new HubConnectionBuilder().WithUrl(HubUrl, HttpConnectionOptionsAction).Build();
            HubConnection.Closed += OnClosed;
            HubConnection.Reconnecting += OnReconnecting;
            HubConnection.Reconnected += OnReconnected;
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
                Output.StatusBar("开始连接通信服务器");
                await HubConnection.StartAsync();
                watch.Stop();
                Output.StatusBar($"通信服务器连接成功，耗时：{watch.Elapsed}");
                object[] args = { new[] { WpfConstants.GroupNameWpf } };
                await SendToHub("AddToGroup", args);
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return new OperationResult(OperationResultType.Error, ex.Message);
            }
        }

        /// <summary>
        /// 停止通信
        /// </summary>
        public virtual async Task Stop()
        {
            if (!IsHubConnected)
            {
                return;
            }
            object[] args = { new[] { WpfConstants.GroupNameWpf } };
            await SendToHub("RemoveFromGroup", args);
            Output.StatusBar("断开通信服务器");
            await HubConnection.StopAsync();
        }

        /// <summary>
        /// 重启通信
        /// </summary>
        /// <returns></returns>
        public virtual async Task<OperationResult> Restart()
        {
            await Stop();
            while (HubConnection.State != HubConnectionState.Disconnected)
            {
                await Task.Delay(50);
            }
            return await Start();
        }

        /// <summary>
        /// 向服务器发送数据
        /// </summary>
        /// <param name="methodName">Hub方法名</param>
        /// <param name="args">调用参数</param>
        /// <returns></returns>
        public virtual async Task SendToHub(string methodName, params object[] args)
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
        public virtual async Task<TResult> RequestFromHub<TResult>(string methodName, params object[] args)
        {
            await WaitForConnected();
            return await HubConnection.InvokeCoreAsync<TResult>(methodName, args);
        }

        /// <summary>
        /// 重写以实现<see cref="HttpConnectionOptions"/>的行为
        /// </summary>
        /// <param name="opts"></param>
        protected virtual void HttpConnectionOptionsAction(HttpConnectionOptions opts)
        {
            if (!string.IsNullOrEmpty(Version))
            {
                opts.Headers["Version"] = Version;
            }
            opts.Headers["HostName"] = Dns.GetHostName();
        }

        /// <summary>
        /// 在连接关闭时触发
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        protected virtual async Task OnClosed(Exception error)
        {
            int delay = Random.Next(0, 5);
            await Output.StatusBarCountdown("{0}秒后重试连接通信服务器", delay);
            await HubConnection.StartAsync();
            Output.StatusBar($"通信服务器连接{(HubConnection.State == HubConnectionState.Connected ? "成功" : "失败")}");
        }

        /// <summary>
        /// 在重连之后触发
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected virtual Task OnReconnected(string arg)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 在正在重连时触发
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected virtual Task OnReconnecting(Exception error)
        {
            return Task.CompletedTask;
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
            // 已连接
            if (IsHubConnected)
            {
                return;
            }

            // 等待正在连接
            while (IsHubConnecting)
            {
                await Task.Delay(200);
                if (IsHubConnected)
                {
                    return;
                }
            }

            while (!IsHubConnected)
            {
                int delay;
                while (!IsNetConnected)
                {
                    delay = Random.Next(6, 12);
                    await Output.StatusBarCountdown("当前网络不通畅，延时{0}秒后重试", delay);
                }

                delay = Random.Next(3, 8);
                await Output.StatusBarCountdown("服务器连接失败，延时{0}秒后重试", delay);
                await Start();
            }
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
        public virtual async Task SendToHub(Expression<Action<TRequest>> call)
        {
            Invocation invocation = call.GetInvocation();
            await WaitForConnected();
            await SendToHub(invocation.MethodName, invocation.Parameters);
        }

        /// <summary>
        /// 向服务器请求并返回数据
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="call">调用委托</param>
        /// <returns></returns>
        public virtual async Task<TResult> RequestFromHub<TResult>(Expression<Func<TRequest, Task<TResult>>> call)
        {
            Invocation invocation = call.GetInvocation();
            await WaitForConnected();
            return await RequestFromHub<TResult>(invocation.MethodName, invocation.Parameters);
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