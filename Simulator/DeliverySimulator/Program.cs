using Common.Model.DataModel;
using Flurl;
using Flurl.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeliverySimulator
{
    class Program
    {
        private const string BaseUrl = "http://localhost:7071";
        private static string deviceId = "d000001";


        static void Main(string[] args)
        {
            Execute().GetAwaiter().GetResult();
        }

        public static async Task Execute()
        {
            Console.WriteLine("Hello World!");

            DeliveryModel deliveryModel = new DeliveryModel()
            {
                Latitude = "",
                Longitude = "",
                DeviceId = "",
                Code = "",
                Status = "",
                CustomerId = "",
                Id = "",
                DriverId = "",
                EndDateTime = DateTime.MinValue,
                StartDateTime = DateTime.MinValue,

            };

            try
            {
                HttpResponseMessage response = await BaseUrl.AppendPathSegment("api").AppendPathSegment("AddDelivery").WithHeader("Accept", "application/json").PostJsonAsync(deliveryModel);
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
                throw;
            }
        }
    }
}
