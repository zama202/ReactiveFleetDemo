using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.DataModel
{
    public class DeviceModel
    {
        [JsonProperty(PropertyName = "id")]
        public string DeviceId { get; set; }
    }
}
