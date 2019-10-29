// -----------------------------------------------------------------------
//  <copyright file="HubManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-28 15:19</last-date>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using OSharp.Core.Options;
using OSharp.Data;
using OSharp.Wpf.Data;


namespace OSharp.Wpf.Net
{
    /// <summary>
    /// 通信连接管理器
    /// </summary>
    public abstract class HubManagerBase
    {
        /// <summary>
        /// 随机对象
        /// </summary>
        protected static readonly Random Random = new Random();

        /// <summary>
        /// 获取 服务器地址
        /// </summary>
        public virtual string HostUrl => AppSettingsReader.GetString("HostUrl");

        /// <summary>
        /// 获取 客户端版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 获取 网络通信连接
        /// </summary>
        public HubConnection HubConnection { get; protected set; }

        /// <summary>
        /// 获取或设置 当前网络是否通畅
        /// </summary>
        public bool IsNetConnected { get; set; } = true;

        /// <summary>
        /// 获取 当前通信是否连接
        /// </summary>
        public bool IsHubConnected { get; private set; }

        /// <summary>
        /// 是否正在重启
        /// </summary>
        protected bool IsRestarting { get; set; }

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
        protected abstract void HubListenOn(HubConnection connection);

        /// <summary>
        /// 开始通信
        /// </summary>
        public async Task<OperationResult> Start()
        {
            try
            {
                Stopwatch watch = Stopwatch.StartNew();
                await HubConnection.StartAsync();
                watch.Stop();
                IsHubConnected = true;
                await Invoke("AddToGroup", new object[] { new[] { WpfConstants.GroupNameWpf } });
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
            await Invoke("RemoveFromGroup", new object[] { new[] { WpfConstants.GroupNameWpf } });
            await HubConnection.StopAsync();
        }

        /// <summary>
        /// 执行无返回数据的通信
        /// </summary>
        public virtual async Task Invoke(string method, params object[] args)
        {
            if (!IsHubConnected)
            {
                await Restart();
            }

            await HubConnection.InvokeCoreAsync(method, args);
        }

        /// <summary>
        /// 执行有返回数据的通信功能
        /// </summary>
        public virtual async Task<TResult> Invoke<TResult>(string method, params object[] args)
        {
            if (!IsHubConnected)
            {
                await Restart();
            }

            return await HubConnection.InvokeCoreAsync<TResult>(method, args);
        }

        /// <summary>
        /// 重启通信
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Restart()
        {
            while (IsRestarting)
            {
                await Task.Delay(1000);
                if (!IsRestarting)
                {
                    return;
                }
            }

            IsRestarting = true;
            while (!IsHubConnected)
            {
                int delay;
                while (!IsNetConnected)
                {
                    delay = Random.Next(6, 12);
                    await Task.Delay(delay * 1000);
                }

                delay = Random.Next(3, 8);
                await Task.Delay(delay * 1000);
                await Start();
            }
            IsRestarting = false;
        }
    }
}