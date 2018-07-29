using Shouldly;

using Xunit;


namespace OSharp.Dependency.Tests
{
    public class ServiceScanOptionsTests
    {
        [Fact]
        public void Ctor_Test()
        {
            ServiceScanOptions options = new ServiceScanOptions();

            options.TransientTypeFinder.ShouldNotBeNull();
            options.ScopedTypeFinder.ShouldNotBeNull();
            options.SingletonTypeFinder.ShouldNotBeNull();

            (options.TransientTypeFinder is TransientDependencyTypeFinder).ShouldBeTrue();
            (options.ScopedTypeFinder is ScopedDependencyTypeFinder).ShouldBeTrue();
            (options.SingletonTypeFinder is SingletonDependencyTypeFinder).ShouldBeTrue();
        }
    }
}
