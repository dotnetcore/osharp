// -----------------------------------------------------------------------
//  <copyright file="AspOsharpPack.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-09 22:20</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Builder;

using OSharp.Core.Packs;


namespace OSharp.AspNetCore
{
    /// <summary>
    ///  ����AspNetCore������Packģ�����
    /// </summary>
    public abstract class AspOsharpPack : OsharpPack
    {
#if NET6_0_OR_GREATER
        /// <summary>
        /// Ӧ��AspNetCore�ķ���ҵ��
        /// </summary>
        /// <param name="app">WebӦ�ó���</param>
        public virtual void UsePack(WebApplication app)
#else
        /// <summary>
        /// Ӧ��AspNetCore�ķ���ҵ��
        /// </summary>
        /// <param name="app">WebӦ�ó��򹹽���</param>
        public virtual void UsePack(IApplicationBuilder app)
#endif
        {
#if NET6_0_OR_GREATER
            IServiceProvider provider = app.Services;
#else
            IServiceProvider provider = app.ApplicationServices;
#endif
            base.UsePack(provider);
        }
    }
}