using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Tracking.Domain;

namespace Service.Tracking.Data
{
    static class Repository
    {
        internal static DeliveryDomain NextDelivery(DocumentClient client, Database database, DocumentCollection collection, ILogger log, string deviceId)
        {
            DeliveryDomain result = null;
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, PartitionKey = new PartitionKey(deviceId) };

            // Now execute the same query via direct SQL
            IQueryable <DeliveryDomain> _query = client.CreateDocumentQuery<DeliveryDomain>(
                UriFactory.CreateDocumentCollectionUri(database.Id, collection.Id),
                $"SELECT * FROM Delivery WHERE Delivery.pk = '{deviceId}' ",
                queryOptions);

            //METTERE UN ORDINAMENTO E UNO STATO
            //_query.ToList();
            Console.WriteLine("Running direct SQL query...");
            foreach (DeliveryDomain delivery in _query)
            {
                result = delivery;
                Console.WriteLine("\tRead {0}", delivery);
            }
            return result;
        }


        internal static async Task<bool> AddDelivery(DocumentClient client, Database database, DocumentCollection collection, ILogger log, DeliveryDomain delivery)
        {
            try
            {
                RequestOptions queryOptions = new RequestOptions { PartitionKey = new PartitionKey(delivery.DeviceId) };
                await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(database.Id, collection.Id, delivery.Id), queryOptions);
                Console.WriteLine("Found {0}", delivery.Id);
                return true;
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    RequestOptions queryOptions = new RequestOptions { PartitionKey = new PartitionKey(delivery.DeviceId) };
                    await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(database.Id, collection.Id), delivery, queryOptions);
                    Console.WriteLine("Created Delivery {0}", delivery.Id);
                    return true;
                }
                else
                {
                    throw;
                }
            }
            
        }
    }
}
