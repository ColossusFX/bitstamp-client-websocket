﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Websocket.Client.Sample.Models;
//
//    var liveTicker = Ticker.FromJson(jsonString);

using System;
using System.Globalization;
using System.Reactive.Subjects;
using Bitstamp.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Bitstamp.Client.Websocket.Responses
{
    public partial class Ticker : ResponseBase
    {
        [JsonProperty("microtimestamp")] public string Microtimestamp { get; set; }

        [JsonProperty("amount")] public double Amount { get; set; }

        [JsonProperty("buy_order_id")] public long BuyOrderId { get; set; }

        [JsonProperty("sell_order_id")] public long SellOrderId { get; set; }

        [JsonProperty("amount_str")] public string AmountStr { get; set; }

        [JsonProperty("price_str")] public string PriceStr { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(LiveTickerParseStringConverter))]
        public long Timestamp { get; set; }

        [JsonProperty("price")] public double Price { get; set; }

        [JsonProperty("type")] public long Type { get; set; }

        [JsonProperty("id")] public long Id { get; set; }

        internal static bool TryHandle(JObject response, ISubject<Ticker> subject)
        {
            if (response?["event"].Value<string>() != "trade") return false;

            var parsed = response.ToObject<Ticker>(BitstampJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;
        }
    }

    public partial class Ticker
    {
        public static Ticker FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Ticker>(json, LiveTickerConverter.Settings);
        }
    }

    public static class LiveTickerSerialize
    {
        public static string ToJson(this Ticker self)
        {
            return JsonConvert.SerializeObject(self, LiveTickerConverter.Settings);
        }
    }

    internal static class LiveTickerConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            }
        };
    }

    internal class LiveTickerParseStringConverter : JsonConverter
    {
        public static readonly LiveTickerParseStringConverter Singleton = new LiveTickerParseStringConverter();

        public override bool CanConvert(Type t)
        {
            return t == typeof(long) || t == typeof(long?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (long.TryParse(value, out l)) return l;
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