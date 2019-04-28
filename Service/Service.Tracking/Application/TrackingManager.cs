using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Model.DataModel;
using Common.Model.EventModel;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Tracking.Data;
using Service.Tracking.Domain;

namespace Service.Tracking.Application
{
    static class TrackingManager
    {
        private static DocumentClient _client;
        private static Database _database;
        private static DocumentCollection _collection;


        internal static async Task<ResponseDomain> GetDeliveryAsync(string body, ILogger log, ExecutionContext context)
        {
            ResponseDomain res;
            InitCosmosClient(context);
            try
            {
                DeviceDomain _device = JsonConvert.DeserializeObject<DeviceDomain>(body);
                DeliveryDomain _delivery = Repository.NextDelivery(_client, _database, _collection, log, _device.DeviceId);
                var json = JsonConvert.SerializeObject(_delivery);
                //var json = new StringContent(_delivery, System.Text.Encoding.UTF8, "application/json");
                res = new ResponseDomain
                {
                    HttpStatus = HttpStatusCode.OK,
                    Body = _delivery
                };

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

        public static async Task<DeliveryDomain> NextDelivery(string deviceId) {
            return await MockDelivery(); 
        }



        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////7
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////7
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////7
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////7

        private static async Task<DeliveryDomain> MockDelivery()
        {
            return await Task.FromResult(new DeliveryDomain
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = "1",
                DeviceId = "DEVICE0001",
                Code = Convert.ToString(Guid.NewGuid()),
                DriverId = "DRIVER0001",
                Status = "OPEN",
                Latitude = "45.650900",
                Longitude = "12.435400"                
            });
        }
       
        //APrire una riga in COSMOS
        internal static void OpenDeliverySession(ActivationEvent activation)
        {
            throw new NotImplementedException();
        }

        //Selezionare una riga in COSMOS e fare upsert
        internal static void CloseDeliverySession(ActivationEvent activation)
        {
            throw new NotImplementedException();
        }

        internal static async Task<ResponseDomain> AddDeliveryAsync(string body, ILogger log, ExecutionContext context)
        {
            ResponseDomain res;
            InitCosmosClient(context);
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

        private static void InitCosmosClient(ExecutionContext context)
        {
            if ((null == _client) || (null == _database) || (null == _collection))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                _client = new DocumentClient(new Uri(config["EndpointUri"]), config["AuthorizationKey"]);
                _database = _client.CreateDatabaseQuery().Where(c => c.Id == config["DatabaseId"]).AsEnumerable().FirstOrDefault(); //ToArray().FirstOrDefault();
                _collection = _client.CreateDocumentCollectionQuery(_database.SelfLink).Where(c => c.Id == config["CollectionId"]).AsEnumerable().FirstOrDefault(); //ToArray().FirstOrDefault();
            }
            
        }
    }




}
