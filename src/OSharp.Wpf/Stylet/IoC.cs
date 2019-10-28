// -----------------------------------------------------------------------
//  <copyright file="IoC.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-29 1:26</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using StyletIoC;


namespace OSharp.Wpf.Stylet
{
    public static class IoC
    {
        public static Func<Type, string, object> GetInstance = (service, key) => throw new InvalidOperationException("IoC is not initialized");

        public static Func<Type, IEnumerable<object>> GetAllInstances = service => throw new InvalidOperationException("IoC is not initialized");

        public static Action<object> BuildUp = instance => throw new InvalidOperationException("IoC is not initialized");

        public static T Get<T>(string key = null)
        {
            return (T)GetInstance(typeof(T), key);
        }

        public static IEnumerable<T> GetAll<T>()
        {
            return GetAllInstances(typeof(T)).Cast<T>();
        }

        public static void Initialize(IContainer container)
        {
            GetInstance = container.Get;
            GetAllInstances = type => container.GetAll(type);
            BuildUp = container.BuildUp;
        }
    }
}