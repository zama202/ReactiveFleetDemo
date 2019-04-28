using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Rest.Azure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Module.EventGridProducer
{
    public class SimpleEventProcessor : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"SimpleEventProcessor initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            var events = new List<EventGridEvent>();

            foreach (var eventData in messages)
            {
                var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                events.Add(getEventGridMessage(data, "position"));
                Console.WriteLine($"Message received. Partition: '{context.PartitionId}', Data: '{data}'");
            }

            if (events.Count > 0)
            {
                sendEventGridMessageWithEventGridClientAsync(topicHostName, events).GetAwaiter().GetResult();
            }


            return context.CheckpointAsync();
        }

        
        private EventGridEvent getEventGridMessage(object data, string subject)
        {
            return new EventGridEvent
            {
                Subject = subject,
                EventType = "func-event",
                EventTime = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Data = data,
                DataVersion = "1.0.0",
            };
        }

        private static string topicEndpoint = "https://<NAME>.eventgrid.azure.net/api/events";
        private static string topicHostName = new Uri(topicEndpoint).Host;
        private static string topicKey = "<KEY>";


        private static async Task sendEventGridMessageWithEventGridClientAsync(string topic, List<EventGridEvent> events)
        {
            TopicCredentials topicCredentials = new TopicCredentials(topicKey);
            EventGridClient client = new EventGridClient(topicCredentials);
            AzureOperationResponse x = await client.PublishEventsWithHttpMessagesAsync(topic, events);
            Console.WriteLine(x.Response.Content);
        }

    }
}
