// -----------------------------------------------------------------------
//  <copyright file="UnitTestBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-10-14 17:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using OSharp.Extensions;


namespace OSharp.UnitTest.Infrastructure
{
    public abstract class EntityTestBase
    {
        protected readonly IEnumerable<TestEntity> Entities;

        protected EntityTestBase()
        {
            List<TestEntity> entities = new List<TestEntity>();
            DateTime dt = DateTime.Now;
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                entities.Add(new TestEntity()
                {
                    Id = i + 1,
                    Name = "Name" + (i + 1),
                    AddDate = rnd.NextDateTime(dt.AddDays(-7), dt.AddDays(7)),
                    IsDeleted = rnd.NextBoolean()
                });
            }
            Entities = entities;
        }
    }
}