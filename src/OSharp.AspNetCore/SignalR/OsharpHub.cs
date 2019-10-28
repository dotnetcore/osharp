// -----------------------------------------------------------------------
//  <copyright file="OsharpHub.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-28 20:51</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;


namespace OSharp.AspNetCore.SignalR
{
    /// <summary>
    /// OsharpHub基类
    /// </summary>
    public abstract class OsharpHub : Hub
    {
        /// <summary>
        /// 初始化一个<see cref="OsharpHub"/>类型的新实例
        /// </summary>
        protected OsharpHub(IConnectionUserCache userCache)
        {
            UserCache = userCache;
        }

        /// <summary>
        /// 获取 通信连接用户缓存
        /// </summary>
        protected IConnectionUserCache UserCache { get; }

        /// <summary>
        /// 在与集线器建立新连接时调用。
        /// </summary>
        /// <returns>一个 <see cref="T:System.Threading.Tasks.Task" /> 表示异步连接的。</returns>
        public override async Task OnConnectedAsync()
        {
            string userName = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                await UserCache.AddConnectionId(userName, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        /// <summary>当终止与集线器的连接时调用。</summary>
        /// <returns>一个 <see cref="T:System.Threading.Tasks.Task" /> 表示异步连接的。</returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userName = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                await UserCache.RemoveConnectionId(userName, Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 加入组
        /// </summary>
        /// <param name="groupNames">组名</param>
        /// <returns></returns>
        public virtual async Task AddToGroup(string[] groupNames)
        {
            foreach (string groupName in groupNames)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }
        }

        /// <summary>
        /// 离开组
        /// </summary>
        /// <param name="groupNames">组名</param>
        /// <returns></returns>
        public virtual async Task RemoveFromGroup(string[] groupNames)
        {
            foreach (string groupName in groupNames)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }
        }
    }


    /// <summary>
    /// 支持强类型的OsharpHub基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class OsharpHub<T> : OsharpHub where T : class
    {
        private IHubCallerClients<T> _clients;

        /// <summary>
        /// 初始化一个<see cref="OsharpHub"/>类型的新实例
        /// </summary>
        protected OsharpHub(IConnectionUserCache userCache)
            : base(userCache)
        { }

        /// <summary>
        /// Gets or sets a <typeparamref name="T" /> that can be used to invoke methods on the clients connected to this hub.
        /// </summary>
        public new IHubCallerClients<T> Clients
        {
            get
            {
                if (this._clients == null)
                    this._clients = (IHubCallerClients<T>)new TypedHubClients<T>(base.Clients);
                return this._clients;
            }
            set
            {
                this._clients = value;
            }
        }

    }


    internal class TypedHubClients<T> : IHubCallerClients<T>, IHubClients<T>
    {
        private readonly IHubCallerClients _hubClients;

        public TypedHubClients(IHubCallerClients dynamicContext)
        {
            this._hubClients = dynamicContext;
        }

        /// <summary>
        /// 所有连接的客户端
        /// </summary>
        public T All
        {
            get
            {
                return TypedClientBuilder<T>.Build(this._hubClients.All);
            }
        }

        /// <summary>
        /// 调用集线器方法的客户端
        /// </summary>
        public T Caller
        {
            get
            {
                return TypedClientBuilder<T>.Build(this._hubClients.Caller);
            }
        }

        /// <summary>
        /// 除当前连接外的所有客户端
        /// </summary>
        public T Others
        {
            get
            {
                return TypedClientBuilder<T>.Build(this._hubClients.Others);
            }
        }

        /// <summary>
        /// 所有连接的客户端（指定的连接除外）
        /// </summary>
        /// <param name="excludedConnectionIds">要排除的多个连接</param>
        /// <returns></returns>
        public T AllExcept(IReadOnlyList<string> excludedConnectionIds)
        {
            return TypedClientBuilder<T>.Build(this._hubClients.AllExcept(excludedConnectionIds));
        }

        /// <summary>
        /// 指定连接的客户端
        /// </summary>
        /// <param name="connectionId">指定连接</param>
        /// <returns></returns>
        public T Client(string connectionId)
        {
            return TypedClientBuilder<T>.Build(this._hubClients.Client(connectionId));
        }

        /// <summary>
        /// 指定名称的组的客户端
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <returns></returns>
        public T Group(string groupName)
        {
            return TypedClientBuilder<T>.Build(this._hubClients.Group(groupName));
        }

        /// <summary>
        /// 指定名称的组并排除指定连接的客户端
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="excludedConnectionIds">排除的连接</param>
        /// <returns></returns>
        public T GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds)
        {
            return TypedClientBuilder<T>.Build(this._hubClients.GroupExcept(groupName, excludedConnectionIds));
        }

        /// <summary>
        /// 指定连接的多个客户端
        /// </summary>
        /// <param name="connectionIds"></param>
        /// <returns></returns>
        public T Clients(IReadOnlyList<string> connectionIds)
        {
            return TypedClientBuilder<T>.Build(this._hubClients.Clients(connectionIds));
        }

        /// <summary>
        /// 指定名称的多个组的客户端
        /// </summary>
        /// <param name="groupNames">多个组名称</param>
        /// <returns></returns>
        public T Groups(IReadOnlyList<string> groupNames)
        {
            return TypedClientBuilder<T>.Build(this._hubClients.Groups(groupNames));
        }

        /// <summary>
        /// 一个组中的客户端，不包括调用该集线器方法的客户端
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <returns></returns>
        public T OthersInGroup(string groupName)
        {
            return TypedClientBuilder<T>.Build(this._hubClients.OthersInGroup(groupName));
        }

        /// <summary>
        /// 指定用户的客户端
        /// </summary>
        /// <param name="userId">用户标识</param>
        /// <returns></returns>
        public T User(string userId)
        {
            return TypedClientBuilder<T>.Build(this._hubClients.User(userId));
        }

        /// <summary>
        /// 指定的多个用户的客户端
        /// </summary>
        /// <param name="userIds">多个用户标识</param>
        /// <returns></returns>
        public T Users(IReadOnlyList<string> userIds)
        {
            return TypedClientBuilder<T>.Build(this._hubClients.Users(userIds));
        }
    }

    internal static class TypedClientBuilder<T>
    {
        private static readonly Lazy<Func<IClientProxy, T>> _builder = new Lazy<Func<IClientProxy, T>>((Func<Func<IClientProxy, T>>)(() => TypedClientBuilder<T>.GenerateClientBuilder()));
        private static readonly PropertyInfo CancellationTokenNoneProperty = typeof(CancellationToken).GetProperty("None", BindingFlags.Static | BindingFlags.Public);
        private const string ClientModuleName = "Microsoft.AspNetCore.SignalR.TypedClientBuilder";

        public static T Build(IClientProxy proxy)
        {
            return TypedClientBuilder<T>._builder.Value(proxy);
        }

        public static void Validate()
        {
            Func<IClientProxy, T> func = TypedClientBuilder<T>._builder.Value;
        }

        private static Func<IClientProxy, T> GenerateClientBuilder()
        {
            TypedClientBuilder<T>.VerifyInterface(typeof(T));
            Type clientType = TypedClientBuilder<T>.GenerateInterfaceImplementation(AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Microsoft.AspNetCore.SignalR.TypedClientBuilder"), AssemblyBuilderAccess.Run).DefineDynamicModule("Microsoft.AspNetCore.SignalR.TypedClientBuilder"));
            return (Func<IClientProxy, T>)(proxy => (T)Activator.CreateInstance(clientType, (object)proxy));
        }

        private static Type GenerateInterfaceImplementation(ModuleBuilder moduleBuilder)
        {
            TypeBuilder type = moduleBuilder.DefineType("Microsoft.AspNetCore.SignalR.TypedClientBuilder." + typeof(T).Name + "Impl", TypeAttributes.Public, typeof(object), new Type[1]
            {
        typeof (T)
            });
            FieldBuilder fieldBuilder = type.DefineField("_proxy", typeof(IClientProxy), FieldAttributes.Private);
            TypedClientBuilder<T>.BuildConstructor(type, (FieldInfo)fieldBuilder);
            foreach (MethodInfo allInterfaceMethod in TypedClientBuilder<T>.GetAllInterfaceMethods(typeof(T)))
                TypedClientBuilder<T>.BuildMethod(type, allInterfaceMethod, (FieldInfo)fieldBuilder);
            return (Type)type.CreateTypeInfo();
        }

        private static IEnumerable<MethodInfo> GetAllInterfaceMethods(
          Type interfaceType)
        {
            Type[] typeArray = interfaceType.GetInterfaces();
            int index;
            for (index = 0; index < typeArray.Length; ++index)
            {
                foreach (MethodInfo allInterfaceMethod in TypedClientBuilder<T>.GetAllInterfaceMethods(typeArray[index]))
                    yield return allInterfaceMethod;
            }
            typeArray = (Type[])null;
            MethodInfo[] methodInfoArray = interfaceType.GetMethods();
            for (index = 0; index < methodInfoArray.Length; ++index)
                yield return methodInfoArray[index];
            methodInfoArray = (MethodInfo[])null;
        }

        private static void BuildConstructor(TypeBuilder type, FieldInfo proxyField)
        {
            MethodBuilder methodBuilder = type.DefineMethod(".ctor", MethodAttributes.Public | MethodAttributes.HideBySig);
            ConstructorInfo constructor = typeof(object).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder)null, new Type[0], (ParameterModifier[])null);
            methodBuilder.SetReturnType(typeof(void));
            methodBuilder.SetParameters(typeof(IClientProxy));
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Call, constructor);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Stfld, proxyField);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void BuildMethod(
          TypeBuilder type,
          MethodInfo interfaceMethodInfo,
          FieldInfo proxyField)
        {
            MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask;
            ParameterInfo[] parameters = interfaceMethodInfo.GetParameters();
            Type[] array1 = ((IEnumerable<ParameterInfo>)parameters).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>)(param => param.ParameterType)).ToArray<Type>();
            MethodBuilder methodBuilder = type.DefineMethod(interfaceMethodInfo.Name, attributes);
            MethodInfo method = typeof(IClientProxy).GetMethod("SendCoreAsync", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder)null, new Type[3]
            {
        typeof (string),
        typeof (object[]),
        typeof (CancellationToken)
            }, (ParameterModifier[])null);
            methodBuilder.SetReturnType(interfaceMethodInfo.ReturnType);
            methodBuilder.SetParameters(array1);
            string[] array2 = ((IEnumerable<Type>)array1).Where<Type>((Func<Type, bool>)(p => p.IsGenericParameter)).Select<Type, string>((Func<Type, string>)(p => p.Name)).Distinct<string>().ToArray<string>();
            if (((IEnumerable<string>)array2).Any<string>())
                methodBuilder.DefineGenericParameters(array2);
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.DeclareLocal(typeof(object[]));
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, proxyField);
            ilGenerator.Emit(OpCodes.Ldstr, interfaceMethodInfo.Name);
            ilGenerator.Emit(OpCodes.Ldc_I4, parameters.Length);
            ilGenerator.Emit(OpCodes.Newarr, typeof(object));
            ilGenerator.Emit(OpCodes.Stloc_0);
            for (int index = 0; index < array1.Length; ++index)
            {
                ilGenerator.Emit(OpCodes.Ldloc_0);
                ilGenerator.Emit(OpCodes.Ldc_I4, index);
                ilGenerator.Emit(OpCodes.Ldarg, index + 1);
                ilGenerator.Emit(OpCodes.Box, array1[index]);
                ilGenerator.Emit(OpCodes.Stelem_Ref);
            }
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Call, TypedClientBuilder<T>.CancellationTokenNoneProperty.GetMethod);
            ilGenerator.Emit(OpCodes.Callvirt, method);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void VerifyInterface(Type interfaceType)
        {
            if (!interfaceType.IsInterface)
                throw new InvalidOperationException("Type must be an interface.");
            if (interfaceType.GetProperties().Length != 0)
                throw new InvalidOperationException("Type must not contain properties.");
            if (interfaceType.GetEvents().Length != 0)
                throw new InvalidOperationException("Type must not contain events.");
            foreach (MethodInfo method in interfaceType.GetMethods())
                TypedClientBuilder<T>.VerifyMethod(interfaceType, method);
            foreach (Type interfaceType1 in interfaceType.GetInterfaces())
                TypedClientBuilder<T>.VerifyInterface(interfaceType1);
        }

        private static void VerifyMethod(Type interfaceType, MethodInfo interfaceMethod)
        {
            if (interfaceMethod.ReturnType != typeof(Task))
                throw new InvalidOperationException("Cannot generate proxy implementation for '" + typeof(T).FullName + "." + interfaceMethod.Name + "'. All client proxy methods must return '" + typeof(Task).FullName + "'.");
            foreach (ParameterInfo parameter in interfaceMethod.GetParameters())
            {
                if (parameter.IsOut)
                    throw new InvalidOperationException("Cannot generate proxy implementation for '" + typeof(T).FullName + "." + interfaceMethod.Name + "'. Client proxy methods must not have 'out' parameters.");
                if (parameter.ParameterType.IsByRef)
                    throw new InvalidOperationException("Cannot generate proxy implementation for '" + typeof(T).FullName + "." + interfaceMethod.Name + "'. Client proxy methods must not have 'ref' parameters.");
            }
        }
    }
}