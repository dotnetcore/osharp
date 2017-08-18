using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OSharp.Reflection;
using Xunit;

namespace OSharp.Tests.Reflection
{
    public class AppDomainAllAssemblyFinderTest
    {
        [Fact]
        public void FindAllTest()
        {
            Assembly[] assembly = new AppDomainAllAssemblyFinder().FindAll();

        }
    }

}
