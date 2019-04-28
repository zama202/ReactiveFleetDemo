using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Common.Model.Enum;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;

namespace Common.Model.EventModel
{
    public class ActivationEvent : MessageEvent
    {
        public ActivationEvent(string deviceId, double latitude, double longitude, string type, string code, string sid)
        {
            PayloadFamily = "av";
            PayloadType = type;
            Position = new Point(new Position(latitude, longitude));
            DeviceId = deviceId;
            EventDate = DateTime.Now;
            Code = code;
            SessionId = sid;
        }

        public ActivationEvent(string deviceId, string latitude, string longitude, string type, string code, string sid)
        {
            PayloadFamily = "av";
            PayloadType = type;
            Position = new Point(new Position(latitude, longitude));
            DeviceId = deviceId;
            EventDate = DateTime.Now;
            Code = code;
            SessionId = sid;
        }

    }
}
