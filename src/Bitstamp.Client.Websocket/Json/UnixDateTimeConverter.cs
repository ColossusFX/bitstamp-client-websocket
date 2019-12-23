﻿using Bitstamp.Client.Websocket.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace Bitstamp.Client.Websocket.Json
{
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var substracted = ((DateTime)value).Subtract(UnixTime.UnixBase);
            writer.WriteRawValue(substracted.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            return UnixTime.ConvertToTime((long)reader.Value);
        }
    }
}