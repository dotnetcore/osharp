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
    public class HubManager
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// 初始化一个<see cref="HubManager"/>类型的新实例
        /// </summary>
        public HubManager(string hostUrl = null)
        {
            HostUrl = AppSettingsReader.GetString("HostUrl") ?? hostUrl;
        }

        /// <summary>
        /// 获取 服务器地址
        /// </summary>
        public string HostUrl { get; }

        /// <summary>
        /// 获取 客户端版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 获取 网络通信连接
        /// </summary>
        public HubConnection HubConnection { get; private set; }

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
        public void Initialize()
        {
            Check.NotNull(HostUrl, nameof(HostUrl));
            HubConnection = new HubConnectionBuilder().WithUrl($"{HostUrl}/assist-hub",
                opts =>
                {
                    opts.Headers["Version"] = Version;
                    opts.Headers["HostName"] = Dns.GetHostName();
                    opts.AccessTokenProvider = () => Task.FromResult(string.Empty);
                }).Build();
            HubConnection.Closed += async error =>
            {
                await Task.Delay(Random.Next(0, 5) * 1000);
                await HubConnection.StartAsync();
            };
        }

        /// <summary>
        /// 开始通信
        /// </summary>
        public async Task<bool> Start()
        {
            try
            {
                Stopwatch watch = Stopwatch.StartNew();
                await HubConnection.StartAsync();
                watch.Stop();
                IsHubConnected = true;
                await Invoke("AddToGroup", new object[] { new[] { WpfConstants.GroupNameWpf } });
            }
            catch (Exception)
            {
                IsHubConnected = false;
                throw;
            }

            return IsHubConnected;
        }

        /// <summary>
        /// 停止通信
        /// </summary>
        public async Task Stop()
        {
            await Invoke("RemoveFromGroup", new object[] { new[] { WpfConstants.GroupNameWpf } });
            await HubConnection.StopAsync();
        }

        /// <summary>
        /// 执行无返回数据的通信
        /// </summary>
        public async Task Invoke(string method, params object[] args)
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
        public async Task<TResult> Invoke<TResult>(string method, params object[] args)
        {
            if (!IsHubConnected)
            {
                await Restart();
            }

            return await HubConnection.InvokeCoreAsync<TResult>(method, args);
        }

        private bool _isRestarting;
        private async Task Restart()
        {
            while (_isRestarting)
            {
                await Task.Delay(1000);
                if (!_isRestarting)
                {
                    return;
                }
            }

            _isRestarting = true;
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
            _isRestarting = false;
        }
    }
}