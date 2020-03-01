// -----------------------------------------------------------------------
//  <copyright file="TokenCleanupHost.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-21 14:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OSharp.Data;
using OSharp.Entity;
using OSharp.IdentityServer.Options;


namespace OSharp.IdentityServer.Services
{
    public class TokenCleanupHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IdentityServerOptions _options;

        private TimeSpan CleanupInterval => TimeSpan.FromSeconds(_options.TokenCleanupInterval);

        /// <summary>
        /// 初始化一个<see cref="TokenCleanupHostedService"/>类型的新实例
        /// </summary>
        public TokenCleanupHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = serviceProvider.GetIdentityServerOptions();
            Check.NotNull(_options, nameof(_options));
        }

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_options.EnableTokenCleanup)
            {
                return;
            }
            while (true)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_options.TokenCleanupInterval), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }

                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                await RemoveExpiredGrantsAsync();
            }
        }

        private async Task RemoveExpiredGrantsAsync()
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IServiceProvider provider = scope.ServiceProvider;

            ITokenCleanupService tokenCleanupService = provider.GetService<ITokenCleanupService>();
            await tokenCleanupService.RemoveExpiredTokenAsync();

            IUnitOfWorkManager unitOfWorkManager = provider.GetService<IUnitOfWorkManager>();
            unitOfWorkManager.Commit();
        }
    }
}