// -----------------------------------------------------------------------
//  <copyright file="PackOutputDto.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-13 14:59</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Core.Packs;
using OSharp.Entity;


namespace OSharp.Hosting.Systems.Dtos
{
    /// <summary>
    /// ���DTO��ģ�����Ϣ
    /// </summary>
    public class PackOutputDto : IOutputDto
    {
        /// <summary>
        /// ��ȡ������ ����
        /// </summary>
        [DisplayName("����")]
        public string Name { get; set; }

        /// <summary>
        /// ��ȡ������ ��ʾ����
        /// </summary>
        [DisplayName("��ʾ����")]
        public string Display { get; set; }

        /// <summary>
        /// ��ȡ������ ����·��
        /// </summary>
        [DisplayName("����·��")]
        public string Class { get; set; }

        /// <summary>
        /// ��ȡ������ ģ�鼶��
        /// </summary>
        [DisplayName("����")]
        public PackLevel Level { get; set; }

        /// <summary>
        /// ��ȡ������ ����˳��
        /// </summary>
        [DisplayName("����˳��")]
        public int Order { get; set; }

        /// <summary>
        /// ��ȡ������ �Ƿ�����
        /// </summary>
        [DisplayName("�Ƿ�����")]
        public bool IsEnabled { get; set; }
    }
}