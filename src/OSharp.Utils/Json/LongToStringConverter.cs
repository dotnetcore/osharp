// -----------------------------------------------------------------------
//  <copyright file="LongToStringConverter.cs" company="LiuliuSoft">
//      Copyright (c) 2022-2022 DaprPlus. All rights reserved.
//  </copyright>
//  <site>https://www.dapr.plus</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2022-11-17 17:11</last-date>
// -----------------------------------------------------------------------

using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace OSharp.Json
{
    /// <summary>
    /// <see cref="long"/>类型转换为<see cref="string"/>类型的转换器
    /// 处理雪花算法生成的Id在前端丢失精度的问题
    /// </summary>
    public class LongToStringConverter : JsonConverter
    {
        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.ToString());
        }

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jt = JToken.ReadFrom(reader);
            var value = jt.Value<long>();
            return value;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(long) || objectType == typeof(ulong);
        }
    }

}
