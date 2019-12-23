﻿using Bitstamp.Client.Websocket.Json;
using Newtonsoft.Json.Linq;
using System.Reactive.Subjects;

namespace Bitstamp.Client.Websocket.Responses
{
    /// <summary>
    ///     Heartbeat response
    /// </summary>
    public class HeartbeatResponse : ResponseBase<HeartbeatResponse>
    {
        internal static bool TryHandle(JObject response, ISubject<HeartbeatResponse> subject)
        {
            if (response?["channel"].Value<string>() != "heartbeat")
            {
                return false;
            }

            var parsed = response.ToObject<HeartbeatResponse>(BitstampJsonSerializer.Serializer);
            subject.OnNext(parsed);
            return true;
        }
    }
}