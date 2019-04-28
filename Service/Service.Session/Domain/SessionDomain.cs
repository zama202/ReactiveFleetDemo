using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Session.Domain
{
    public class SessionDomain
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

        [JsonProperty("slng")]
        public string StartLongitude { get; set; }

        [JsonProperty("slat")]
        public string StartLatitude { get; set; }

        [JsonProperty("elng")]
        public string EndLongitude { get; set; }

        [JsonProperty("elat")]
        public string EndLatitude { get; set; }

        [JsonProperty("sts")]
        public DateTime StartDateTime { get; set; }

        [JsonProperty("ets")]
        public DateTime EndDateTime { get; set; }




    }
}
