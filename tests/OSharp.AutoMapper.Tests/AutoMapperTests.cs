using System;

using AutoMapper.Configuration;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Mapping;
using OSharp.UnitTest.Infrastructure;

using Xunit;

namespace OSharp.AutoMapper.Tests
{
    public class AutoMapperTests
    {
        private readonly ServiceProvider _provider;

        public AutoMapperTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IMapTuple, TestProfile>();
            services.AddOSharp().AddPack<AutoMapperPack>();
            _provider = services.BuildServiceProvider();
            _provider.UseOsharp();
        }

        [Fact]
        public void Test01()
        {
            DateTime dt = new DateTime(2021, 4, 7, 1, 2, 3, 4);
            TestEntity entity = new TestEntity() { Id = 22, Name = "TestName", AddDate = dt };
            TestDto dto = entity.MapTo<TestDto>();
            Assert.Equal(22, dto.Id);
            Assert.Equal(dt, dto.CreatedTime);
        }
    }


    [MapFrom(typeof(TestEntity))]
    public class TestDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool IsDeleted { get; set; }
    }


    public class TestProfile : AutoMapperTupleBase
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        public override void CreateMap()
        {
            CreateMap<TestEntity, TestDto>().ForMember(d => d.CreatedTime, opt => opt.MapFrom(e => e.AddDate));
        }
    }
}
