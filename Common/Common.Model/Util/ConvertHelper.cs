using System;
using System.Collections.Generic;
using System.Text;
using AzureMapsToolkit.Common;
using Common.Model.DataModel;
using Common.Model.EventModel;
using Newtonsoft.Json;

namespace Common.Util
{
    public class ConvertHelper
    {
        public static object Duration { get; private set; }

        public static void ToTripModel(RouteResultLeg routeResultLeg, out TripModel model)
        {
            model = new TripModel {
                Points = routeResultLeg.Points,
                Duration = routeResultLeg.Summary.TravelTimeInSeconds,
                Distance = routeResultLeg.Summary.LengthInMeters
            };

        }

        public static DeviceModel GetDeviceModel(string body)
        {
            return JsonConvert.DeserializeObject<DeviceModel>(body);
        }

        public static StatusModel GetStatusModel(string body)
        {
            return JsonConvert.DeserializeObject<StatusModel>(body);
        }
    }
}
