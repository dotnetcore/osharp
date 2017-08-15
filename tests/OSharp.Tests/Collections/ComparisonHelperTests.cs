using Xunit;

using System.Collections.Generic;
using System.Linq;


namespace OSharp.Collections.Tests
{
    public class ComparisonHelperTests
    {
        [Fact()]
        public void CreateComparerTest()
        {
            List<int> list2 = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                list2.Add(i);
            }
            List<int> list1 = new List<int>();
            for (int i = 50; i < 150; i++)
            {
                list1.Add(i);
            }
            IComparer<int> comparer = ComparisonHelper<int>.CreateComparer(m => m);
            List<int> list3 = list1.Union(list2).ToList();
            Assert.NotEqual(list3[0], 0);
            Assert.NotEqual(list3[149], 149);
            list3.Sort(comparer);
            Assert.Equal(list3[0], 0);
            Assert.Equal(list3[149], 149);
        }
    }
}