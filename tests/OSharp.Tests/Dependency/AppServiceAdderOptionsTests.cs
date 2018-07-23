using Shouldly;

using Xunit;


namespace OSharp.Dependency.Tests
{
    public class AppServiceAdderOptionsTests
    {
        [Fact]
        public void Ctor_Test()
        {
            AppServiceAdderOptions options = new AppServiceAdderOptions();

            options.TransientTypeFinder.ShouldNotBeNull();
            options.ScopedTypeFinder.ShouldNotBeNull();
            options.SingletonTypeFinder.ShouldNotBeNull();

            (options.TransientTypeFinder is TransientDependencyTypeFinder).ShouldBeTrue();
            (options.ScopedTypeFinder is ScopedDependencyTypeFinder).ShouldBeTrue();
            (options.SingletonTypeFinder is SingletonDependencyTypeFinder).ShouldBeTrue();
        }
    }
}
