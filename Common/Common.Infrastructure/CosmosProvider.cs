using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Common.Infrastructure
{
    public class CosmosProvider
    {
        public readonly DocumentClient cosmosClient;
        public readonly Database cosmosDatabase;
        public readonly DocumentCollection cosmosCollection;

        public CosmosProvider(DocumentClient cosmosClient, Database cosmosDatabase, DocumentCollection cosmosCollection)
        {
            this.cosmosClient = cosmosClient;
            this.cosmosDatabase = cosmosDatabase;
            this.cosmosCollection = cosmosCollection;
        }

        public static DocumentClient CosmosClient { get; set; }
        public static Database CosmosDatabase { get; set; }
        public static DocumentCollection CosmosCollection { get; set; }

        public static CosmosProvider Init(string EndpointUri, string AuthorizationKey, string DatabaseId, string CollectionId)
        {

            if (CosmosClient == null || CosmosDatabase == null || CosmosCollection == null)
            {
                CosmosClient = new DocumentClient(new Uri(EndpointUri), AuthorizationKey);
                CosmosDatabase = CosmosClient.CreateDatabaseQuery().Where(c => c.Id == DatabaseId).AsEnumerable().FirstOrDefault();
                CosmosCollection = CosmosClient.CreateDocumentCollectionQuery(CosmosDatabase.SelfLink).Where(c => c.Id == CollectionId).AsEnumerable().FirstOrDefault();
            }


            return new CosmosProvider(CosmosClient, CosmosDatabase, CosmosCollection);
        }

        

    }
}
