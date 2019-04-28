using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Model.EventModel;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Session.Data;
using Service.Session.Domain;

namespace Service.Session.Application
{
    public class SessionManager
    {
        //Selezionare una riga in COSMOS e fare upsert
        public static async Task<bool> CloseSessionAsync(string body, ILogger log, ExecutionContext context)
        {
            ActivationEvent activation = JsonConvert.DeserializeObject<ActivationEvent>(body);

            throw new NotImplementedException();
        }

        //APrire una riga in COSMOS
        public static async Task<ResponseDomain> OpenSessionAsync(string body, ILogger log, ExecutionContext context)
        {
            ResponseDomain res;
            InitCosmosClient(context);
            try
            {
                ActivationEvent _activation = JsonConvert.DeserializeObject<ActivationEvent>(body);
                bool result = await Repository.AddSessionDelivery(_client, _database, _collection, log, _activation);

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
