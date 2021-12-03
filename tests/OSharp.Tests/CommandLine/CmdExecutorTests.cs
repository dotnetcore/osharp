using Xunit;
using OSharp.CommandLine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shouldly;


namespace OSharp.CommandLine.Tests
{
    public class CmdExecutorTests
    {
        [Fact()]
        public void ExecuteCommandTest()
        {
            string output = CmdExecutor.ExecuteCmd("dotnet --info");
            output.ShouldContain("5.0");
        }

        [Fact()]
        public void ExecuteCmdFileTest()
        {
            string file = "CommandLine/test.bat";
            string output = CmdExecutor.ExecuteCmdFile(file);
            output.ShouldContain("5.0");
        }
    }
}