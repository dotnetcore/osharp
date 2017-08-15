using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Collections;
using OSharp.Filter;
using OSharp.UnitTest.Infrastructure;
using Xunit;

namespace OSharp.Tests.Collections
{
    public class QueryableExtensionsTests
    {
        [Fact()]
        public void WhereIfTest_IQueryable()
        {
            IQueryable<int> source = new List<int> { 1, 2, 3, 4, 5, 6, 7 }.AsQueryable();
            Assert.Equal(source.WhereIf(m => m > 5, false).ToList(), source.ToList());

            List<int> actual = new List<int> { 6, 7 };
            Assert.Equal(source.WhereIf(m => m > 5, true).ToList(), actual);
        }

        [Fact()]
        public void OrderByTest_IQueryable()
        {
            IQueryable<TestEntity> source = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "abc" },
                new TestEntity { Id = 4, Name = "fda", IsDeleted = true },
                new TestEntity { Id = 6, Name = "rwg", IsDeleted = true },
                new TestEntity { Id = 3, Name = "hdg" },
            }.AsQueryable();

            Assert.Equal(source.OrderBy("Id").ToArray()[1].Name, "hdg");
            Assert.Equal(source.OrderBy("Name", ListSortDirection.Descending).ToArray()[3].Id, 1);
            Assert.Equal(source.OrderBy(new SortCondition("Id")).ToArray()[1].Name, "hdg");
            Assert.Equal(source.OrderBy(new SortCondition<TestEntity>(m => m.Id)).ToArray()[1].Name, "hdg");
            Assert.Equal(source.OrderBy(new SortCondition<TestEntity>(m => m.Name.Length, ListSortDirection.Ascending)).ToArray()[1].Name, "fda");
            Assert.Equal(source.OrderBy(new SortCondition("Name", ListSortDirection.Descending)).ToArray()[3].Id, 1);
        }

        [Fact()]
        public void ThenByTest_IQueryable()
        {
            IQueryable<TestEntity> source = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "abc" },
                new TestEntity { Id = 4, Name = "fda", IsDeleted = true },
                new TestEntity { Id = 6, Name = "rwg", IsDeleted = true },
                new TestEntity { Id = 3, Name = "hdg" },
            }.AsQueryable();
            Assert.Equal(source.OrderBy("IsDeleted").ThenBy("Id").ToArray()[2].Name, "fda");
            Assert.Equal(source.OrderBy("IsDeleted", ListSortDirection.Descending).ThenBy("Id", ListSortDirection.Descending).ToArray()[2].Name,
                "hdg");
            Assert.Equal(source.OrderBy(new SortCondition("IsDeleted")).ThenBy(new SortCondition("Name")).ToArray()[2].Name, "fda");
        }
    }
}
