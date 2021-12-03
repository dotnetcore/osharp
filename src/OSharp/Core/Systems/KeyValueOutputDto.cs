// -----------------------------------------------------------------------
//  <copyright file="KeyValueOutputDto.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-25 21:35</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.Core.Systems
{
    /// <summary>
    /// ���DTO:��ֵ����
    /// </summary>
    [MapFrom(typeof(KeyValue))]
    public class KeyValueOutputDto : IOutputDto, IDataAuthEnabled
    {
        /// <summary>
        /// ��ȡ������ ���
        /// </summary>
        [DisplayName("���")]
        public Guid Id { get; set; }

        /// <summary>
        /// ��ȡ������ ����ֵJSON
        /// </summary>
        [DisplayName("����ֵJSON")]
        public string ValueJson { get; set; }

        /// <summary>
        /// ��ȡ������ ����ֵ������
        /// </summary>
        [DisplayName("����ֵ������")]
        public string ValueType { get; set; }

        /// <summary>
        /// ��ȡ������ ���ݼ���
        /// </summary>
        [DisplayName("���ݼ���")]
        public string Key { get; set; }

        /// <summary>
        /// ��ȡ������ ����ֵ
        /// </summary>
        [DisplayName("����ֵ")]
        public object Value { get; set; }

        /// <summary>
        /// ��ȡ������ �Ƿ�����
        /// </summary>
        [DisplayName("�Ƿ�����")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// ��ȡ������ �Ƿ�ɸ��µ�����Ȩ��״̬
        /// </summary>
        public bool Updatable { get; set; }

        /// <summary>
        /// ��ȡ������ �Ƿ��ɾ��������Ȩ��״̬
        /// </summary>
        public bool Deletable { get; set; }
    }
}