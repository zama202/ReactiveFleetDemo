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
using System.Net;

namespace Trigger.Delivery
{
    public static class DeliveryEndpoint
    {
        [FunctionName("AddDelivery")]
        public static async Task<HttpResponseMessage> AddDelivery([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, ILogger log, ExecutionContext context)
        {
            //Read and Deserialize
            var body = await req.Content.ReadAsStringAsync();
            log.LogInformation($"HttpHook hooked: {body}");
            //Call AppManager and get the result
            var result = await DeliveryManager.AddDeliveryAsync(body, log, context);
            //Build Response
            return req.CreateResponse(result.HttpStatus, result.Body);
        }

        [FunctionName("GetDelivery")]
        public static async Task<HttpResponseMessage> GetDelivery([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, ILogger log, ExecutionContext context)
        {
            //Read and Deserialize
            var body = await req.Content.ReadAsStringAsync();
            log.LogInformation($"HttpHook hooked: {body}");
            //Call AppManager and get the result
            var result = await DeliveryManager.GetDeliveryAsync(body, log, context);
            //Build Response
            return req.CreateResponse(result.HttpStatus, result.Body, new JsonMediaTypeFormatter());
        }


        /// //////////////////////////////////////////
        /// //////////////////////////////////////////
        /// //////////////////////////////////////////
        /// //////////////////////////////////////////



        //Delivery fallita per cliente assente, riordino 
        [FunctionName("AbortDelivery")]
        public static void AbortDelivery([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
        }


        [FunctionName("CloseDelivery")]
        public static void CloseDelivery([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var body = eventGridEvent.Data.ToString();
            log.LogInformation($"EventGridTriggerHook hooked: {body}");

            //Do the stuff
            var result = DeliveryManager.CloseDeliveryAsync(body, log).GetAwaiter().GetResult();
            log.LogInformation(eventGridEvent.Data.ToString());

            //Scateno il Container con Statistiche
        }

        [FunctionName("CloseDeliveryApi")]
        public static async Task<HttpResponseMessage> CloseDeliveryApi([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, ILogger log, ExecutionContext context)
        {
            //Read and Deserialize
            var body = await req.Content.ReadAsStringAsync();
            log.LogInformation($"EventGridTriggerHook hooked: {body}");

            //Do the stuff
            var result = DeliveryManager.CloseDeliveryAsync(body, log).GetAwaiter().GetResult();

            //Build Response
            return req.CreateResponse(result.HttpStatus, result.Body, new JsonMediaTypeFormatter());
        }

        [FunctionName("OpenDelivery")]
        public static void OpenDelivery([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var body = eventGridEvent.Data.ToString();
            log.LogInformation($"EventGridTriggerHook hooked: {body}");

            //Do the stuff
            var result = DeliveryManager.OpenDeliveryAsync(body, log).GetAwaiter().GetResult();
            log.LogInformation(eventGridEvent.Data.ToString());

            //Scateno il Container con Statistiche
        }

        [FunctionName("OpenDeliveryApi")]
        public static async Task<HttpResponseMessage> OpenDeliveryApi([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, ILogger log, ExecutionContext context)
        {
            //Read and Deserialize
            var body = await req.Content.ReadAsStringAsync();
            log.LogInformation($"EventGridTriggerHook hooked: {body}");

            //Do the stuff
            var result = DeliveryManager.OpenDeliveryAsync(body, log).GetAwaiter().GetResult();

            //Build Response
            return req.CreateResponse(result.HttpStatus, result.Body, new JsonMediaTypeFormatter());
        }



        [FunctionName("InjectDelivery")]
        public static void InjectDelivery([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            //TODO
            log.LogInformation(eventGridEvent.Data.ToString());
            //INJECT via IotHub come DirectMethod
        }

        

        [FunctionName("SetDeliveryStatus")]
        public static async Task<HttpResponseMessage> SetDelivery([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            var body = await req.Content.ReadAsStringAsync();
            log.LogInformation($"HttpHook hooked: {body}");
            return req.CreateResponse(HttpStatusCode.OK, new { greeting = "hello" });
        }
    }
}
