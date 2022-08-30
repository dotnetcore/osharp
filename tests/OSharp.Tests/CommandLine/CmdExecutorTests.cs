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
            if (!OperatingSystem.IsWindows())
            {
                return;
            }
            string output = CmdExecutor.ExecuteCmd("dotnet --info");
            output.ShouldContain("6.0");
        }

        [Fact()]
        public void ExecuteCmdFileTest()
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }
            string file = "CommandLine/test.bat";
            if (!File.Exists(file))
            {
                return;
            }
            string output = CmdExecutor.ExecuteCmdFile(file);
            output.ShouldContain("dotnet\\sdk");
        }
    }
}
