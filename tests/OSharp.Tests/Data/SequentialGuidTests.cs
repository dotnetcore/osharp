using Xunit;
using OSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shouldly;


namespace OSharp.Data.Tests
{
    public class SequentialGuidTests
    {
        [Fact()]
        public void NewGuidTest1()
        {
            List<Guid> ids = new List<Guid>();
            for (int i = 0; i < 100; i++)
            {
                ids.Add(SequentialGuid.Create(SequentialGuidType.SequentialAtEnd));
            }
            ids.Distinct().Count().ShouldBe(100);
            ids.Clear();

            for (int i = 0; i < 100; i++)
            {
                ids.Add(SequentialGuid.Create(SequentialGuidType.SequentialAsString));
            }
            ids.Distinct().Count().ShouldBe(100);
            ids.Clear();

            for (int i = 0; i < 100; i++)
            {
                ids.Add(SequentialGuid.Create(SequentialGuidType.SequentialAsBinary));
            }
            ids.Distinct().Count().ShouldBe(100);
            ids.Clear();
        }
    }
}