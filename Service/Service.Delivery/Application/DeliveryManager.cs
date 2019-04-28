using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Infrastructure;
using Common.Model.EventModel;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Delivery.Data;
using Service.Delivery.Domain;

namespace Service.Delivery.Application
{
    public static class DeliveryManager
    {
        private static DocumentClient _client;
        private static Database _database;
        private static DocumentCollection _collection;


        private static string EndpointUri = "https://<COSMOSACCOUNT>.documents.azure.com:443/";
        private static string AuthorizationKey = "SECRET";
        private static string DatabaseId = "DDB";
        private static string CollectionId = "DC";
        private static string PrimaryKey = "pk";
        
        public static async Task<ResponseDomain> GetDeliveryAsync(string body, ILogger log, ExecutionContext context)
        {
            ResponseDomain res;
            InitCosmosClient();

            try
            {
                DeviceDomain _device = JsonConvert.DeserializeObject<DeviceDomain>(body);
                DeliveryDomain _delivery = Repository.NextDelivery(_client, _database, _collection, log, _device.DeviceId);

                if (null != _delivery)
                {
                    var json = JsonConvert.SerializeObject(_delivery);
                    //var json = new StringContent(_delivery, System.Text.Encoding.UTF8, "application/json");
                    res = new ResponseDomain
                    {
                        HttpStatus = HttpStatusCode.OK,
                        Body = Enumerable.Repeat(0, 1).Select(h => _delivery).ToArray()
                    };
                }
                else
                {
                    res = new ResponseDomain
                    {
                        HttpStatus = HttpStatusCode.NotFound,
                        Body = new List<string>()
                    };
                }

                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                res = new ResponseDomain
                {
                    Body = "",
                    HttpStatus = HttpStatusCode.InternalServerError
                };
            }
            return res;
        }

        public static async Task<ResponseDomain> CloseDeliveryAsync(string body, ILogger log)
        {
            ResponseDomain res;
            InitCosmosClient();
            try
            {
                bool result = false;
                StreamEvent msg = JsonConvert.DeserializeObject<StreamEvent>(body);
                if (msg.pf == "av" && msg.pt == "2")
                    result = await Repository.CloseDelivery(_client, _database, _collection, log, msg.pk, msg.code);



                

                if (result)
                    res = new ResponseDomain { HttpStatus = HttpStatusCode.Created, Body = "{}" };
                else
                    res = new ResponseDomain { HttpStatus = HttpStatusCode.BadRequest, Body = "{}" };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                res = new ResponseDomain
                {
                    Body = "{}",
                    HttpStatus = HttpStatusCode.InternalServerError
                };
            }
            return res;
        }

        public static async Task<ResponseDomain> OpenDeliveryAsync(string body, ILogger log)
        {
            ResponseDomain res;
            InitCosmosClient();
            try
            {
                bool result = false;
                StreamEvent msg = JsonConvert.DeserializeObject<StreamEvent>(body);
                if (msg.pf == "av" && msg.pt == "1")
                    result = await Repository.OpenDelivery(_client, _database, _collection, log, msg.pk, msg.code);
                
                if (result)
                    res = new ResponseDomain { HttpStatus = HttpStatusCode.Created, Body = "{}" };
                else
                    res = new ResponseDomain { HttpStatus = HttpStatusCode.BadRequest, Body = "{}" };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                res = new ResponseDomain
                {
                    Body = "{}",
                    HttpStatus = HttpStatusCode.InternalServerError
                };
            }
            return res;
        }


        public static async Task<ResponseDomain> AddDeliveryAsync(string body, ILogger log, ExecutionContext context)
        {
            ResponseDomain res;
            InitCosmosClient();
            try
            {
                DeliveryDomain _delivery = JsonConvert.DeserializeObject<DeliveryDomain>(body);
                bool result = await Repository.AddDelivery(_client, _database, _collection, log, _delivery);

                if (result)
                    res = new ResponseDomain { HttpStatus = HttpStatusCode.Created, Body = "{}" };
                else
                    res = new ResponseDomain { HttpStatus = HttpStatusCode.BadRequest, Body = "{}" };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                res = new ResponseDomain
                {
                    Body = "{}",
                    HttpStatus = HttpStatusCode.InternalServerError
                };
            }
            return res;
        }

        private static void InitCosmosClient()
        {
            if ((null == _client) || (null == _database) || (null == _collection))
            {
                CosmosProvider provider = CosmosProvider.Init(EndpointUri, AuthorizationKey, DatabaseId, CollectionId);
                _client = provider.cosmosClient;
                _database = provider.cosmosDatabase;
                _collection = provider.cosmosCollection;
                
            }
            
        }
    }
}
