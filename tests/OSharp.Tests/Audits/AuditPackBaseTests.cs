using Xunit;
using OSharp.Audits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Packs;

using Shouldly;


namespace OSharp.Audits.Tests
{
    public class AuditPackBaseTests
    {
        [Fact()]
        public void AddServicesTest()
        {
            IServiceCollection services = new ServiceCollection();
            AuditPack pack = new AuditPack();
            pack.Level.ShouldBe(PackLevel.Application);
            pack.Order.ShouldBe(0);
            pack.AddServices(services);
            services.Count.ShouldNotBe(0);
            services.ShouldNotBeEmpty();
            services.ShouldContain(m => m.ServiceType == typeof(AuditEntityEventHandler) && m.Lifetime == ServiceLifetime.Transient);
        }


        class AuditPack : AuditPackBase
        { }
    }
}