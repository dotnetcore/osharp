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

        public YitterSnowKeyGenerator()
        {
        }

        public long Create()
        {
            return YitIdHelper.NextId();
        }
    }
}
