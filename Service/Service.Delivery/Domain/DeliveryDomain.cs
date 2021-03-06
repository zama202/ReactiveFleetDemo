﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Delivery.Domain
{
    public class DeliveryDomain
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

        [JsonProperty("sts")]
        public DateTime StartDateTime { get; set; }

        [JsonProperty("ets")]
        public DateTime EndDateTime { get; set; }




    }
}
