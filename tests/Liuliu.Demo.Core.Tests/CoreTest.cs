using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmResolver.DotNet;

using Xunit;


namespace Liuliu.Demo.Core.Tests
{
    public class CoreTest
    {
        [Fact]
        public void Test01()
        {
            string path = "Liuliu.Demo.Core.dll";
            var moduleDef = ModuleDefinition.FromFile(path);
            foreach (TypeDefinition typeDef in moduleDef.GetAllTypes())
            {
                
            }
        }
    }
}
