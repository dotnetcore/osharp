using Xunit;


namespace OSharp.Filter.Tests
{
    public class FilterGroupTests
    {
        [Fact()]
        public void FilterGroupTest()
        {
            FilterGroup group = new FilterGroup();
            Assert.Equal(FilterOperate.And, group.Operate);
            Assert.NotNull(group.Rules);
            Assert.NotNull(group.Groups);
        }
    }
}