using Xunit;
using OSharp.Data.Snows;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSharp.Data.Snows.Tests
{
    public class IdHelperTests
    {
        [Fact()]
        public void NextIdTest()
        {
            int count = 10000;
            List<long> list = new List<long>();
            for (int i = 0; i < count; i++)
            {
                list.Add(IdHelper.NextId());
            }

            Assert.Equal(list.Count, count);
            Assert.True(list.Distinct().Count() == count);
        }

        [Fact()]
        public void SetIdGeneratorTest()
        {
            IdHelper.SetIdGenerator(new IdGeneratorOptions(1) { Method = 1 });
            string id1 = IdHelper.NextId().ToString();

            IdHelper.SetIdGenerator(new IdGeneratorOptions(1){Method = 2});
            string id2 = IdHelper.NextId().ToString();
            
            Assert.True(id1.Length == id2.Length);
        }
    }
}
