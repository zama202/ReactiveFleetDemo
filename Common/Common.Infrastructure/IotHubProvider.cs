﻿using Microsoft.Azure.Devices;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure
{
    public static class IotHubProvider
    {
        private static ServiceClient s_serviceClient = null;

        // Connection string for your IoT Hub
        private readonly static string s_connectionString = "HostName=gab2019vrihs001.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=s6lkEmIzpn6g8hPYwu5160RXeAFAeOZSu1F+uJ2ihWM=";

        // Invoke the direct method on the device, passing the payload
        public static async Task<int> InvokeMethod(string deviceId, string methodName, string content)
        {
            var methodInvocation = new CloudToDeviceMethod(methodName) { ResponseTimeout = TimeSpan.FromSeconds(30) };
            methodInvocation.SetPayloadJson(JsonConvert.SerializeObject(content));

            // Invoke the direct method asynchronously and get the response from the simulated device.
            var response = await s_serviceClient.InvokeDeviceMethodAsync(deviceId, methodInvocation);

            Console.WriteLine("Response status: {0}, payload:", response.Status);
            Console.WriteLine(response.GetPayloadAsJson());

            return response.Status;
        }

        public static ServiceClient GetClient()
        {
            if (s_serviceClient == null)
                s_serviceClient = ServiceClient.CreateFromConnectionString(s_connectionString);
            return s_serviceClient;
        }

    }
}
