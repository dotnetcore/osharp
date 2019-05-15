// -----------------------------------------------------------------------
//  <copyright file="FastInvokeHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//      Copyfrom: http://www.codeproject.com/Articles/14593/A-General-Fast-Method-Invoker
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-11-19 8:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Reflection.Emit;


namespace OSharp.Reflection
{
    using InvokeHandler = Func<object, object[], object>;


    /// <summary>
    /// 快速执行处理器
    /// </summary>
    public class FastInvokeHandler
    {
        /// <summary>
        /// 创建方法的快速处理封装
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static InvokeHandler Create(MethodInfo methodInfo)
        {
            if (methodInfo.DeclaringType == null)
            {
                throw new InvalidOperationException("methodInfo的类型为空。");
            }
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty,
                typeof(object),
                new[] { typeof(object), typeof(object[]) },
                methodInfo.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                }
                else
                {
                    paramTypes[i] = ps[i].ParameterType;
                }
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];

            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(ps[i].ParameterType.IsByRef ? OpCodes.Ldloca_S : OpCodes.Ldloc, locals[i]);
            }
            il.EmitCall(methodInfo.IsStatic ? OpCodes.Call : OpCodes.Callvirt, methodInfo, null);
            if (methodInfo.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else
            {
                EmitBoxIfNeeded(il, methodInfo.ReturnType);
            }

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                    {
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    }
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            il.Emit(OpCodes.Ret);
            InvokeHandler invoker = (InvokeHandler)dynamicMethod.CreateDelegate(typeof(InvokeHandler));
            return invoker;
        }

        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            il.Emit(type.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, type);
        }

        private static void EmitBoxIfNeeded(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }


            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }
    }
}