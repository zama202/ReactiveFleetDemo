using System;
using System.Collections.Generic;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;
using Flurl;
using Flurl.Http;
using AzureMapsToolkit.Common;
using Common.Model.Enum;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Common.Model.EventModel;
using Common.Model.DataModel;

namespace TruckSimulator
{
    static class Program
    {

        private const string BaseUrl = "https://gab2019vrafa003triggerdelivery.azurewebsites.net";
        private static DeviceClient _deviceClient;
        private static string deviceId = "d000001";

        public static Position CurrentPosition = new Position(45.5794400, 12.3733300);

        public static void Main(string[] args)
        {
            

            try
            {

                //GetConfiguration
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();

                //Initialize Device Client
                GetClient(configuration, new HandlerConfig().WithElement("SetDelivery", SetDelivery).Elements);

                //Execute the stuff
                Execute().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception: {ex.ToString()}");
            }

        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        public static async Task Execute()
        {

            while (true)
            {
                DeliveryModel[] deliveryModel = null;
                //GetLastPosition
                //TODO
                //GetDElivery
                try
                {
                    deliveryModel = await BaseUrl.AppendPathSegment("api").AppendPathSegment("GetDelivery").WithHeader("Accept", "application/json").PostJsonAsync(new { id = "d000001" }).ReceiveJson<DeliveryModel[]>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw;
                }

                if (null != deliveryModel)
                {
                    RouteResultLeg leg = await GetLeg(deliveryModel[0]);
                    int tickTime = leg.Summary.TravelTimeInSeconds / leg.Points.Length;

                    Console.WriteLine($"tickTime: {tickTime}");
                    string SessionId = Guid.NewGuid().ToString();

                    if (leg.Points.Length > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("DELIVERY No." + deliveryModel[0].Code + " is started");
                        
                        await SendEvent(new ActivationEvent(deviceId, GetCurrentPosition().Latitude, GetCurrentPosition().Longitude, Convert.ToString((int)ActivationType.Type.Started), deliveryModel[0].Code, SessionId));
                        Console.WriteLine($"Latitudine: {GetCurrentPosition().Latitude}, longitudine: {GetCurrentPosition().Longitude} with tickTime: {tickTime} seconds");
                        Thread.Sleep(100);
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("LENGTH: "+ leg.Points.Length);
                    Console.ForegroundColor = ConsoleColor.White;
                    //StartSendData
                    for (int i = 0; i < leg.Points.Length; i++)
                    {
                        

                        await SendEvent(new PositionEvent(deviceId, leg.Points[i].Latitude, leg.Points[i].Longitude, deliveryModel[0].Code, SessionId));
                        Console.WriteLine($"Latitudine: {leg.Points[i].Latitude}, longitudine: {leg.Points[i].Longitude} with tickTime: {tickTime} seconds");
                        Thread.Sleep(100 * tickTime);

                        SetCurrentPosition(new Position(leg.Points[i].Latitude, leg.Points[i].Longitude));
                        
                    }

                    if (leg.Points.Length > 0)
                    {
                        await SendEvent(new ActivationEvent(deviceId, GetCurrentPosition().Latitude, GetCurrentPosition().Longitude, Convert.ToString((int)ActivationType.Type.Stopped), deliveryModel[0].Code, SessionId));
                        Console.WriteLine($"Latitudine: {GetCurrentPosition().Latitude}, longitudine: {GetCurrentPosition().Longitude} with tickTime: {tickTime} seconds");
                        Thread.Sleep(100);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("DELIVERY No." + deliveryModel[0].Code + " is ended");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    //AskFroDelivery
                    //SendDeliveryChanged/Storing the last Position)
                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(5000);
                }
                
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static async Task SendEvent(BaseEvent payload)
        {

            var messageString = JsonConvert.SerializeObject(payload);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            // Add a custom application property to the message.
            // An IoT hub can filter on these properties without access to the message body.
            //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

            // Send the tlemetry message
            await _deviceClient.SendEventAsync(message).ConfigureAwait(false);
            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
            await Task.Delay(500);
        }

        private static void GetClient(IConfigurationRoot configuration, SortedList<string, MethodCallback> Handlers)
        {
            Console.WriteLine("start");

            var ConnectionString = configuration.GetSection("ConnectionString")["IotHub"];
            // Create a connection using device context for AMQP session
            _deviceClient = DeviceClient.CreateFromConnectionString(ConnectionString, TransportType.Amqp);
            Console.WriteLine(ConnectionString);

            foreach (KeyValuePair<string, MethodCallback> handler in Handlers)
            {
                _deviceClient.SetMethodHandlerAsync(handler.Key, handler.Value, null).Wait();
            }
        }

        private static Task<RouteResultLeg> GetLeg(DeliveryModel deliveryModel)
        {
            return GetSession(Convert.ToString(GetCurrentPosition().Latitude).Replace(",", "."), Convert.ToString(GetCurrentPosition().Longitude).Replace(",", "."), deliveryModel.Latitude, deliveryModel.Longitude);
        }


        private static Position GetCurrentPosition()
        {
            return CurrentPosition;
        }
        private static void SetCurrentPosition(Position position)
        {
            CurrentPosition = position;
        }


        public static async Task<RouteResultLeg> GetSession(string start_lat, string start_lng, string end_lat, string end_lng)
        {
            var am = new AzureMapsToolkit.AzureMapsServices("hl5yAHeRE4I3q_ZDT-tFiTG0lfDARF7pauzaowCJGdQ");

            RouteResultLeg leg = null;

            RouteRequestDirections r = new RouteRequestDirections
            {
                Query = $"{start_lat},{start_lng}:{end_lat},{end_lng}",
                ApiVersion = "1.0",
                MaxAlternatives = 1
            };

            var resp = await am.GetRouteDirections(r);
            if (resp.Error != null)
            {
                //Handle error
            }
            else
                leg = resp.Result.Routes[0].Legs[0];
            return leg;
        }


        // The device connection string to authenticate the device with your IoT hub.
        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDevice --output table
        //private readonly static string s_connectionString = "{Your device connection string here}";



        // Async method to send simulated telemetry
        /*
                private static async void SendDeviceToCloudMessagesAsync()
                {
                    // Initial telemetry values
                    double minTemperature = 20;
                    double minHumidity = 60;
                    Random rand = new Random();
                    //await GetWorkOrderAsync();

                    while (true)
                    {
                        double currentTemperature = minTemperature + rand.NextDouble() * 15;
                        double currentHumidity = minHumidity + rand.NextDouble() * 20;

                        // Create JSON message
                        var telemetryDataPoint = new
                        {
                            temperature = currentTemperature,
                            humidity = currentHumidity
                        };
                        var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                        var message = new Message(Encoding.ASCII.GetBytes(messageString));

                        // Add a custom application property to the message.
                        // An IoT hub can filter on these properties without access to the message body.
                        message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                        // Send the tlemetry message
                        await _deviceClient.SendEventAsync(message).ConfigureAwait(false);
                        Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                        await Task.Delay(s_telemetryInterval * 1000);

                    }
                }*/

        private async static Task<MethodResponse> SetDelivery(MethodRequest methodRequest, object userContext)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Received Command SetDelivery");
            string data = Encoding.UTF8.GetString(methodRequest.Data);
            string result = "{\"result\":\"Executed direct method: " + methodRequest.Name + "\"}";
            Console.WriteLine(result + " || " + data);
            Console.ResetColor();
            return await Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));

            /*

            // Check the payload is a single integer value
            if (Int32.TryParse(data.Replace("\"", ""), out s_telemetryInterval))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Telemetry interval set to {0} seconds", data);
                Console.ResetColor();

                // Acknowlege the direct method call with a 200 success message
                string result = "{\"result\":\"Executed direct method: " + methodRequest.Name + "\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));
            }
            else
            {
                // Acknowlege the direct method call with a 400 error message
                string result = "{\"result\":\"Invalid parameter\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 400));
            }
            */


//            return await Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(""), 200));
        }
    }

    public class HandlerConfig
    {
        public SortedList<string, MethodCallback> Elements { get; set; } = new SortedList<string, MethodCallback>();

        public HandlerConfig WithElement(string methodName, MethodCallback methodImplementation)
        {
            Elements.Add(methodName, methodImplementation);
            return this;
        }
    }

}