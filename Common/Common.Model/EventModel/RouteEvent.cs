using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.EventModel
{
    public class RouteEvent : BaseEvent
    {
        [JsonProperty("plat")]
        public string StatusLatitude { get; set; }
        [JsonProperty("plng")]
        public string StatusLongitude { get; set; }
        [JsonProperty("nlat")]
        public string GoalLatitude { get; set; }
        [JsonProperty("nlng")]
        public string GoalLongitude { get; set; }

    }
}
