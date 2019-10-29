// -----------------------------------------------------------------------
//  <copyright file="HubConnectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-29 13:27</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using OSharp.Reflection;


namespace OSharp.Wpf.Hubs
{
    /// <summary>
    /// <see cref="HubConnection"/>扩展方法
    /// </summary>
    public static class HubConnectionExtensions
    {
        /// <summary>
        /// HubConnection使用强类型实例进行监听
        /// </summary>
        public static void On<T>(this HubConnection connection, T instance)
        {
            MethodInfo[] methods = instance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methods.Where(m => m.ReturnType == typeof(Task)))
            {
                On(connection, instance, method);
            }
        }

        private static IDisposable On<T>(HubConnection connection, T instance, MethodInfo method)
        {
            Type[] paramTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
            Func<object, object[], object> handler = FastInvokeHandler.Create(method);
            return connection.On(method.Name, paramTypes, args => Task.Run(() => handler.Invoke(instance, args)));
        }
    }
}