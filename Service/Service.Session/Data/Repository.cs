using Common.Model.EventModel;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Session.Data
{
    public class Repository
    {
        public static Task<bool> AddSessionDelivery(DocumentClient client, Database database, DocumentCollection collection, ILogger log, ActivationEvent activation)
        {
            throw new NotImplementedException();
        }
    }
}
