using GeoJSON.Net;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Model.EventModel
{
    public class BaseEvent
    {
        [JsonProperty(PropertyName = "pk")]
        public string DeviceId { get; set; }

    }
}
