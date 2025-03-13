using System;
using Microsoft.Extensions.Options;
using OSharp.Entity.KeyGenerate;
using OSharp.Data.Snows;
using Yitter.IdGenerator;
using IdGeneratorOptions = Yitter.IdGenerator.IdGeneratorOptions;

namespace Liuliu.Demo.Web.Startups.Yitter
{
    public class YitterSnowKeyGenerator : IKeyGenerator<long>
    {
        private readonly IdGeneratorOptions _options;
        private readonly ILogger<YitterSnowKeyGenerator> _logger;

        public YitterSnowKeyGenerator(IOptions<IdGeneratorOptions> options, ILogger<YitterSnowKeyGenerator> logger)
        {
            _logger = logger;   
            _options = options.Value;
            InitializeIdGenerator();
        }

        private void InitializeIdGenerator()
        {
            var options = new IdGeneratorOptions
            {
                Method = _options.Method,
                BaseTime = _options.BaseTime,
                WorkerId = _options.WorkerId,
                WorkerIdBitLength = _options.WorkerIdBitLength,
                SeqBitLength = _options.SeqBitLength,
                MaxSeqNumber = _options.MaxSeqNumber,
                MinSeqNumber = _options.MinSeqNumber,
                TopOverCostCount = _options.TopOverCostCount
            };
            var _workerId = _options.WorkerId;
            _logger.LogInformation($"Yitter.IdGenerator已配置，WorkerId: {_workerId}");
            YitIdHelper.SetIdGenerator(options);
        }

        public long Create()
        {
            return YitIdHelper.NextId();
        }
    }
}
