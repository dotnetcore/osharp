using System;
using System.Collections.Generic;
using System.IO;

using Xunit;


namespace OSharp.Data.Tests
{
    public class CheckTests
    {
        private const string Name = "name";

        [Fact]
        public void Required_Test()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Check.Required(true, null, "message");
            });
            Check.Required(true, m => true, "message");

            Assert.Throws<ArgumentNullException>(() =>
            {
                Check.Required<string, ArgumentNullException>("abc", null, "message");
            });
            Assert.Throws<NullReferenceException>(() =>
            {
                Check.Required<string, NullReferenceException>((string)null, m => m.Length > 0, "message");
            });
        }

        [Fact]
        public void NotNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Check.NotNull<string>(null, Name);
            });
            string value = "abc";
            Check.NotNull(value, nameof(value));
        }

        [Fact]
        public void NotNullOrEmpty_String_Test()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Check.NotNullOrEmpty(null, Name);
            });
            Assert.Throws<ArgumentException>(() =>
            {
                Check.NotNullOrEmpty("", Name);
            });
            Check.NotNullOrEmpty("abc", Name);
        }

        [Fact]
        public void NotEmpty_Guid_Test()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Check.NotEmpty(Guid.Empty, Name);
            });
            Check.NotEmpty(Guid.NewGuid(), Name);
        }

        [Fact]
        public void NotNullOrEmpty_Collection_Test()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Check.NotNullOrEmpty(new List<int>(), Name);
            });
            Check.NotNullOrEmpty(new List<int>() { 1 }, Name);
        }

        [Fact]
        public void LessThan_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Check.LessThan(0, Name, 0, false);
            });
            Check.LessThan(0, Name, 0, true);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Check.LessThan(1, Name, 0, false);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Check.LessThan(1, Name, 0, true);
            });
            Check.LessThan(-1, Name, 0, false);
            Check.LessThan(-1, Name, 0, true);
        }

        [Fact]
        public void GreaterThan_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Check.GreaterThan(0, Name, 0, false);
            });
            Check.GreaterThan(0, Name, 0, true);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Check.GreaterThan(-1, Name, 0, false);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Check.GreaterThan(-1, Name, 0, true);
            });
            Check.GreaterThan(1, Name, 0, false);
            Check.GreaterThan(1, Name, 0, true);
        }

        [Fact]
        public void Between_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Check.Between(0, Name, 0, 0, false, false);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Check.Between(0, Name, 0, 0, true, false);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Check.Between(0, Name, 0, 0, false, true);
            });
            Check.Between(0, Name, 0, 0, true, true);
            Check.Between(0, Name, -1, 1, true, true);
        }

        [Fact]
        public void DirectoryExists_Test()
        {
            Assert.Throws<DirectoryNotFoundException>(() =>
            {
                Check.DirectoryExists(Directory.GetCurrentDirectory() + "A");
            });
            Check.DirectoryExists(Directory.GetCurrentDirectory());
        }

        [Fact]
        public void FileExists_Test()
        {
            Assert.Throws<FileNotFoundException>(() =>
            {
                Check.FileExists("OSharp.Tests1.dll");
            });
            Check.FileExists("OSharp.Tests.dll");
        }
    }
}
