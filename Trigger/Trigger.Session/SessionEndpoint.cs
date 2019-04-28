using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Service.Session.Application;

namespace Trigger.Session
{
    public static class SessionEndpoint
    {
        [FunctionName("OpenSession")]
        public static void OpenSession([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log, ExecutionContext context)
        {
            try
            {
                var body = eventGridEvent.Data.ToString();
                log.LogInformation($"EventGridTriggerHook hooked: {body}");

                //Do the stuff
                var result = SessionManager.OpenSessionAsync(body, log);
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

        [FunctionName("CloseSession")]
        public static void run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log, ExecutionContext context)
        {
            try
            {
                var body = eventGridEvent.Data.ToString();
                log.LogInformation($"EventGridTriggerHook hooked: {body}");

                //Do the stuff
                var result = SessionManager.CloseSession(body, log);
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
    }
}
