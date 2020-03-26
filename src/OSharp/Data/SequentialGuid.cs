// -----------------------------------------------------------------------
//  <copyright file="SequentialGuidGenerator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-26 10:02</last-date>
// -----------------------------------------------------------------------

using System;
using System.Security.Cryptography;

using OSharp.Entity;


namespace OSharp.Data
{
    /// <summary>
    /// 有序GUID生成器
    /// 源自：https://github.com/jhtodd/SequentialGuid/blob/master/SequentialGuid/Classes/SequentialGuid.cs
    /// </summary>
    public static class SequentialGuid
    {
        /// <summary>
        /// 为GUID的创建提供加密强随机数据。
        /// </summary>
        private static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();

        /// <summary>
        /// 生成指定类型的GUID
        /// </summary>
        public static Guid Create(SequentialGuidType guidType)
        {
            byte[] randomBytes = new byte[10];
            Rng.GetBytes(randomBytes);

            long timestamp = DateTime.UtcNow.Ticks / 10000L;
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            byte[] guidBytes = new byte[16];
            switch (guidType)
            {
                case SequentialGuidType.SequentialAsString:
                case SequentialGuidType.SequentialAsBinary:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    // If formatting as a string, we have to reverse the order
                    // of the Data1 and Data2 blocks on little-endian systems.
                    if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }
                    break;
                case SequentialGuidType.SequentialAtEnd:
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(guidType), guidType, null);
            }

            return new Guid(guidBytes);
        }

        /// <summary>
        /// 生成指定数据库类型的有序GUID
        /// </summary>
        public static Guid Create(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    return Create(SequentialGuidType.SequentialAtEnd);
                case DatabaseType.Sqlite:
                case DatabaseType.MySql:
                case DatabaseType.PostgreSql:
                    return Create(SequentialGuidType.SequentialAsString);
                case DatabaseType.Oracle:
                    return Create(SequentialGuidType.SequentialAsBinary);
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null);
            }
        }
    }
}