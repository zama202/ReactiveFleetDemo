using System;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Infrastructure;
using Confluent.Kafka;
using Common.Model.EventModel;
using Confluent.Kafka.Serialization;
using System.Text;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Azure.EventHubs;
using System.Threading;

namespace Module.EventGridProducer
{
    class Program
    {
       

        private const string EventHubConnectionString = "<NAMESPACECONN>";
        private const string EventHubName = "TOPICNAME";
        private const string StorageContainerName = "CHECKPOINTSTORAGE_CONTAINER";
        private const string StorageAccountName = "CHECKPOINT_ACCOUNTNAME";
        private const string StorageAccountKey = "CHECKPOINT_ACCOUNTPASSWORD";

        private static readonly string StorageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Registering EventProcessor...");

            var eventProcessorHost = new EventProcessorHost(
                EventHubName,
                PartitionReceiver.DefaultConsumerGroupName,
                EventHubConnectionString,
                StorageConnectionString,
                StorageContainerName);

            // Registers the Event Processor Host and starts receiving messages
            await eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>();

            Thread.Sleep(Timeout.Infinite);

            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }
}
