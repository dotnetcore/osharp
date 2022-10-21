// -----------------------------------------------------------------------
//  <copyright file="IComClassFactory.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-05 11:12</last-date>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;


namespace OSharp.Reflection
{
    /// <summary>
    /// Com类型实例工厂
    /// </summary>
    [ComVisible(true), ComImport, Guid("00000001-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComClassFactory
    {
        /// <summary>
        /// CreateInstance
        /// </summary>
        /// <param name="pUnkOuter"></param>
        /// <param name="riid"></param>
        /// <param name="obj">创建输出的对象</param>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int CreateInstance([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter,
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out, MarshalAs(UnmanagedType.Interface)] out object obj);

        /// <summary>
        /// LockServer
        /// </summary>
        /// <param name="fLock"></param>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int LockServer([MarshalAs(UnmanagedType.Bool), In] bool fLock);
    }
}