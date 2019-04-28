using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Tracking.Domain
{
    public class StatusDomain
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("uid")]
        public string DriverId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("cid")]
        public string CustomerId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("pk")]
        public string DeviceId { get; set; }

        [JsonProperty("lng")]
        public string Longitude { get; set; }

        [JsonProperty("lat")]
        public string Latitude { get; set; }

        [JsonProperty("lts")]
        public DateTime LastDateTime { get; set; }
        

    }
}
