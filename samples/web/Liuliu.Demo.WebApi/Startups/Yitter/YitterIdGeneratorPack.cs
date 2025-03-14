using System.ComponentModel;
using OSharp.Hosting.Identity.Entities;
using OSharp.Hosting.MultiTenancy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSharp.Authorization;
using OSharp.Authorization.Modules;
using OSharp.Caching;
using OSharp.Core.Packs;
using OSharp.Core.Systems;
using OSharp.Entity;
using OSharp.Identity;
using Yitter.IdGenerator;
using Liuliu.Demo.Web.Startups.Yitter;
using OSharp.Entity.KeyGenerate;
using System.Configuration;
using Microsoft.Extensions.Options;
using OSharp.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.DotNet.Scaffolding.Shared;

namespace Liuliu.Demo.Web.Startups.Yitter
{
    [DependsOnPacks(typeof(OsharpCorePack))]
    public class YitterIdGeneratorPack : OsharpPack
    {
        private const string MainLockName = "sys_idGen:workerId:lock";
        private const string MainValueKey = "sys_idGen:workerId:value";

        public YitterIdGeneratorPack()
        {
        }

        public override PackLevel Level => PackLevel.Framework;

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();
            services.Configure<IdGeneratorOptions>(configuration.GetSection("IdGeneratorOptions"));               

            services.Replace<IKeyGenerator<long>, YitterSnowKeyGenerator>(ServiceLifetime.Singleton);
            return services;
        }

        override public void UsePack(IServiceProvider provider)
        {
            var _options = provider.GetRequiredService<IOptions<IdGeneratorOptions>>().Value;
            var _logger = provider.GetRequiredService<ILogger<YitterIdGeneratorPack>>();
            var workerId = GetWorkerIdAsync(provider, _options).Result;
            var options = new IdGeneratorOptions
            {
                Method = _options.Method,
                BaseTime = _options.BaseTime,
                WorkerId = workerId,
                WorkerIdBitLength = _options.WorkerIdBitLength,
                SeqBitLength = _options.SeqBitLength,
                MaxSeqNumber = _options.MaxSeqNumber,
                MinSeqNumber = _options.MinSeqNumber,
                TopOverCostCount = _options.TopOverCostCount
            };
            
            _logger.LogInformation($"Yitter.IdGenerator已配置，WorkerId: {workerId}");
            YitIdHelper.SetIdGenerator(options);
        }

        private async Task<ushort> GetWorkerIdAsync(IServiceProvider provider,IdGeneratorOptions options)
        {
            var lockName = $"{MainLockName}";
            var valueKey = $"{MainValueKey}";

            var minWorkId = 0;
            var maxWorkId = (int)Math.Pow(2, (double)options.WorkerIdBitLength);

            var cache = provider.GetRequiredService<IDistributedCache>();
            var _logger = provider.GetRequiredService<ILogger<YitterIdGeneratorPack>>();

            long workId = -1;
            var tempWorkIds = Enumerable.Range(minWorkId, maxWorkId).Select(id => id.ToString()).ToList();
            using (var distributedLock = new DistributedLock(cache, lockName))
            {
                if (!await distributedLock.AcquireAsync(TimeSpan.FromSeconds(10)))
                {
                    throw new OsharpException("获取分布式锁失败");
                }
                try
                {
                    string workIdKey = "";
                    foreach (var item in tempWorkIds)
                    {
                        var workIdStr = item;
                        workIdKey = $"{valueKey}:{workIdStr}";
                        var exist = cache.Get<bool>(workIdKey);
                        if (exist)
                        {
                            workIdKey = "";
                            continue;
                        }

                        _logger.LogInformation($"############ 当前应用雪花WorkId:【{workIdStr}】############");

                        workId = long.Parse(workIdStr);
                        if (workId < minWorkId || workId > maxWorkId)
                            continue;

                        // 设置雪花Id算法机器码
                        YitIdHelper.SetIdGenerator(new IdGeneratorOptions
                        {
                            WorkerId = (ushort)workId,
                            WorkerIdBitLength = options.WorkerIdBitLength,
                            SeqBitLength = options.SeqBitLength
                        });

                        var cacheOptions = new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                        };
                        cache.Set(workIdKey, true, cacheOptions);

                        break;
                    }

                    if (string.IsNullOrWhiteSpace(workIdKey)) throw new OsharpException("未设置有效的机器码，启动失败");

                    // 开一个任务设置当前workId过期时间
                    _ = Task.Run(() =>
                    {
                        while (true)
                        {
                            var cacheOptions = new DistributedCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                            };
                            cache.Set(workIdKey, true, cacheOptions);

                            Thread.Sleep(10000);
                        }
                    });
                }
                catch (Exception ex)
                {
                    throw new OsharpException($"{ex.Message};{ex.StackTrace};{ex.StackTrace}");
                }
            }
            if(workId < minWorkId || workId >= maxWorkId) throw new OsharpException("未设置有效的机器码，启动失败");
            return (ushort)workId;
        }
    }
}
