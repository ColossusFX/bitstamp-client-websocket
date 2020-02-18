﻿using System;
using Newtonsoft.Json;

namespace Bitstamp.Client.Websocket.Responses
{
    /// <summary>
    /// Message which is used as base for every request and response
    /// </summary>
    public abstract class ResponseBase<T> where T : new()
    {
        public T Data { get; set; }
        public string Event { get; set; }
        public string Channel { get; set; }
        public virtual string Symbol { get; set; }
    }

    internal class EventStringConverter : JsonConverter
    {
        public static readonly EventStringConverter Singleton = new EventStringConverter();

        public override bool CanConvert(Type t)
        {
            return t == typeof(long) || t == typeof(long?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            var value = serializer.Deserialize<string>(reader);
            if (long.TryParse(value, out var l)) return l;

            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            var value = (long) untypedValue;
            serializer.Serialize(writer, value.ToString());
        }
    }
}