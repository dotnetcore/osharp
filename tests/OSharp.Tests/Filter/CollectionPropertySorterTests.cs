using Xunit;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using OSharp.UnitTest.Infrastructure;


namespace OSharp.Filter.Tests
{
    public class CollectionPropertySorterTests:EntityTestBase
    {
        [Fact()]
        public void OrderByTest()
        {
            IEnumerable<TestEntity> list1 = Entities.ToList();
            IEnumerable<TestEntity> list2 = CollectionPropertySorter<TestEntity>.OrderBy(list1, "AddDate", ListSortDirection.Ascending);
            Assert.False(list2.SequenceEqual(list1));
            IOrderedEnumerable<TestEntity> temp1 = CollectionPropertySorter<TestEntity>.OrderBy(list1, "AddDate", ListSortDirection.Ascending);
            IEnumerable<TestEntity> list3 = CollectionPropertySorter<TestEntity>.ThenBy(temp1, "IsDeleted", ListSortDirection.Descending);
            Assert.False(list1.SequenceEqual(list3));

            IQueryable<TestEntity> query1 = Entities.AsQueryable();
            IQueryable<TestEntity> query2 = CollectionPropertySorter<TestEntity>.OrderBy(query1, "AddDate", ListSortDirection.Ascending);
            Assert.False(query2.SequenceEqual(query1));
            IOrderedQueryable<TestEntity> temp2 = CollectionPropertySorter<TestEntity>.OrderBy(query1, "AddDate", ListSortDirection.Ascending);
            IEnumerable<TestEntity> query3 = CollectionPropertySorter<TestEntity>.ThenBy(temp2, "IsDeleted", ListSortDirection.Descending);
            Assert.False(query1.SequenceEqual(query3));
        }
    }
}