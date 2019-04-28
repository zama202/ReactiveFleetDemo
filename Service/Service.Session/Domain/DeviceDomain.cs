using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Session.Domain
{
    public class DeviceDomain
    {
        [JsonProperty(PropertyName = "id")]
        public string DeviceId { get; set; }
    }
}
