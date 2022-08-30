using Xunit;
using OSharp.CommandLine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

using Shouldly;


namespace OSharp.CommandLine.Tests
{
    [SupportedOSPlatform("windows")]
    public class CmdExecutorTests
    {
        [Fact()]
        public void ExecuteCommandTest()
        {
            string output = CmdExecutor.ExecuteCmd("dotnet --info");
            output.ShouldContain("6.0");
        }

        [Fact()]
        public void ExecuteCmdFileTest()
        {
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
