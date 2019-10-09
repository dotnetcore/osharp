using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace OSharp.Collections.Tests
{
    public class EqualityHelperTests
    {
        [Fact()]
        public void CreateComparerTest()
        {
            List<int> list1 = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                list1.Add(i);
            }
            List<int> list2 = new List<int>();
            for (int i = 50; i < 150; i++)
            {
                list2.Add(i);
            }
            IEqualityComparer<int> comparer = EqualityHelper<int>.CreateComparer(m => m);
            List<int> list3 = list1.Intersect(list2, comparer).ToList();
            Assert.Equal(50, list3.Count);
            Assert.Equal(50, list3.Min());
            Assert.Equal(99, list3.Max());
            List<int> list4 = list1.Except(list2, comparer).ToList();
            Assert.Equal(50, list4.Count);
            Assert.Equal(0, list4.Min());
            Assert.Equal(49, list4.Max());

            EqualityHelper<int>.CreateComparer(m => m, comparer);

        }

    }
}