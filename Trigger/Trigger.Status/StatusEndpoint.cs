// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Service.Status.Application;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Trigger.Status
{
    public static class StatusEndpoint
    {
        //Entra in azione su POSIZIONI
        //Entra in azione su ATTIVAZIONI


        [FunctionName("SetStatus")]
        public static void SetStatus([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            try
            {
                var body = eventGridEvent.Data.ToString();
                log.LogInformation($"EventGridTriggerHook hooked: {body}");

                //Do the stuff
                var result = StatusManager.SetStatusAsync(body, log).GetAwaiter().GetResult();
                //Notify
                //TODO
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            log.LogInformation(eventGridEvent.Data.ToString());
           
        }

        [FunctionName("GetStatus")]
        public static async Task<HttpResponseMessage> GetStatus([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, ILogger log)
        {

            var body = await req.Content.ReadAsStringAsync();
            log.LogInformation($"HttpHook hooked: {body}");
            var result = await StatusManager.GetStatusAsync(body, log);
            return req.CreateResponse(result.HttpStatus, result.Body);
        }

        [FunctionName("SetStatusApi")]
        public static async Task<HttpResponseMessage> SetStatusApi([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, ILogger log)
        {

            try
            {
                var body = await req.Content.ReadAsStringAsync();
                log.LogInformation($"HttpHook hooked: {body}");

                //Do the stuff
                var result = StatusManager.SetStatusAsync(body, log).GetAwaiter().GetResult();
                //Notify
                //TODO
                return req.CreateResponse(result.HttpStatus, result.Body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            
            
        }
    }
}
