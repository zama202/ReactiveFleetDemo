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
       

        private const string EventHubConnectionString = "Endpoint=sb://gab2019vrehn001.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=7s7LYUr0kGRflc0xQi74c0XgT4lMV9D82OYLpeEvKco=";
        private const string EventHubName = "gab2019vreht001";
        private const string StorageContainerName = "gab2019vreht001cp";
        private const string StorageAccountName = "gab2019vrvhd001";
        private const string StorageAccountKey = "saZxlXNQhlD+3DlxwDwhZJZuXMY5YOnQW+elNvl6qsilguPIvi0RhiDkh8/ihgOO92GBBhQIxbQmeMJvvXlM2w==";

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






/*
 *
 {
	"ed": "2019-04-25T13:38:11.7108488+02:00",
	"pf": "av",
	"pt": "1",
	"sid": "48373419-b583-4d72-876a-8f72a6917c0f",
	"code": "db8cbaf5-64fb-46e0-bd15-f09ffb30d980",
	"pos": {
		"type": "Point",
		"coordinates": [12.37333, 45.57944]
	},
	"data": null,
	"pk": "d000001"
}
 *
 *
 * *
 *
 * */

