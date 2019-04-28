using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Status.Domain
{
    class StatusDomain
    {
        [JsonProperty(PropertyName = "pk")]
        public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "uid")]
        public string DriverId { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

    }
}
