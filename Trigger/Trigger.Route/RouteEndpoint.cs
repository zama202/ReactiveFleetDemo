using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System.Net.Http.Formatting;
using Service.Delivery.Application;
using Common.Model.EventModel;
using Common.Model.DataModel;
using Common.Util;
using Common.Model.Provider;
using System.Net;

namespace Trigger.Route
{
    public static class RouteEndpoint
    {
        [FunctionName("SetRoute")]
        public static void SetRoute([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
        }
        

        [FunctionName("GetRoute")]
        public static async Task<HttpResponseMessage> GetRoute([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            //Read and Deserialize
            var body = await req.Content.ReadAsStringAsync();
            RouteEvent routeObj = JsonConvert.DeserializeObject<RouteEvent>(body);

            //Do the stuff
            TripModel trip;
            ConvertHelper.ToTripModel(await MapService.getRoute(routeObj), out trip);

            //Convert to output
            var json = JsonConvert.SerializeObject(trip, Formatting.Indented);
            return req.CreateResponse(HttpStatusCode.OK, trip);
        }

        [FunctionName("SetRoute")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
        }
    }
}
