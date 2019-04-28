using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;

namespace Common.Model.EventModel
{
    public class PositionEvent : MessageEvent
    {
        public PositionEvent(string deviceId, double latitude, double longitude, string code, string sid)
        {
            PayloadFamily = "p";
            PayloadType = "1";
            Position = new Point(new Position(latitude, longitude));
            DeviceId = deviceId;
            EventDate = DateTime.Now;
            Code = code;
            SessionId = sid;
        }

        public PositionEvent(string deviceId, string latitude, string longitude, string code, string sid)
        {
            PayloadFamily = "p";
            PayloadType = "1";
            Position = new Point(new Position(latitude, longitude));
            DeviceId = deviceId;
            EventDate = DateTime.Now;
            Code = code;
            SessionId = sid;
        }
    }
}
