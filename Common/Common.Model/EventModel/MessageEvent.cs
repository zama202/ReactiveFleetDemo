using GeoJSON.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.EventModel
{
    public class MessageEvent : BaseEvent
    {

        [JsonProperty(PropertyName = "ed")]
        public DateTime EventDate { get; set; }

        [JsonProperty(PropertyName = "pf")]
        public string PayloadFamily { get; set; }

        [JsonProperty(PropertyName = "pt")]
        public string PayloadType { get; set; }

        [JsonProperty(PropertyName = "sid")]
        public string SessionId { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "pos")]
        public GeoJSONObject Position { get; set; }

        [JsonProperty(PropertyName = "data")]
        public JObject Data { get; set; }
    }
}
