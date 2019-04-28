using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Rest.Azure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure
{

    public static class EventGridProvider
    {
        private static readonly string topicEndpoint = "https://<NAME>.eventgrid.azure.net/api/events";
        private static readonly string topicHostName = new Uri(topicEndpoint).Host;
        private static readonly string topicKey = "<KEY>";
        private static EventGridClient client = null;

        public static EventGridClient GetClient()
        {
            if (client == null)
            {
                TopicCredentials topicCredentials = new TopicCredentials(topicKey);
                client = new EventGridClient(topicCredentials);
            }
            return client;
        }

        public static async Task<HttpContent> SendAsync(EventGridClient client, string topic, string subject, object data)
        {
            var eventGridEvent = new EventGridEvent
            {
                Subject = subject,
                EventType = "func-event",
                EventTime = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Data = data,
                DataVersion = "1.0.0",
            };

            var events = new List<EventGridEvent>
            {
                eventGridEvent
            };

            Task.Delay(TimeSpan.FromSeconds(3)).GetAwaiter().GetResult();

            AzureOperationResponse x = await client.PublishEventsWithHttpMessagesAsync(topic, events);
            return (x.Response.Content);
        }
    }

}