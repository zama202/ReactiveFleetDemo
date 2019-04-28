using Common.Infrastructure;
using Common.Util;
using Microsoft.Extensions.Logging;
using Service.Status.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Model.DataModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Common.Model.EventModel;

namespace Service.Status.Application
{
    public static class StatusManager
    {
        public static async Task<ResponseDomain> GetStatusAsync(string body, ILogger log)
        {
            ResponseDomain res;
            try
            {
                var status = await RedisProvider.GetValue(RedisProvider.getConnectionAsync().GetAwaiter().GetResult().conn, 0, ConvertHelper.GetDeviceModel(body).DeviceId);

                if (null != status)
                {
                    StatusModel model = JsonConvert.DeserializeObject<StatusModel>(status.ToString());

                    res = new ResponseDomain
                    {
                        HttpStatus = HttpStatusCode.OK,
                        Body = Enumerable.Repeat(0, 1).Select(h => model).ToArray()
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


        public static async Task<ResponseDomain> SetStatusAsync(string body, ILogger log)
        {
            ResponseDomain res;
            try
            {
                StreamEvent msg = JsonConvert.DeserializeObject<StreamEvent>(body);


                var status = await RedisProvider.GetValue(RedisProvider.getConnectionAsync().GetAwaiter().GetResult().conn, 0, msg.pk);

                StatusModel model;

                if (null == status)
                    model = new StatusModel();
                else
                    model = JsonConvert.DeserializeObject<StatusModel>(status.ToString());
                Console.WriteLine("msg: " + msg);

                if (null != status)
                    Console.WriteLine("status: " + status.ToString());
                else
                    Console.WriteLine("status is null");

                if (msg.pf == "p")
                {
                    StreamEvent p = JsonConvert.DeserializeObject<StreamEvent>(body);
                    model.Latitude = p.pos.coordinates[0] + "";
                    model.Longitude = p.pos.coordinates[1] + "";
                    model.LastDateTime = DateTime.Now;
                    model.DeviceId = p.pk;
                    model.Code = p.code;
                }

                if (msg.pf == "av")
                {
                    StreamEvent av = JsonConvert.DeserializeObject<StreamEvent>(body);
                    model.Latitude = av.pos.coordinates[0] + "";
                    model.Longitude = av.pos.coordinates[1] + "";
                    model.LastDateTime = DateTime.Now;
                    model.DeviceId = av.pk;
                    model.Code = av.code;
                    model.Status = av.pt;
                }

                bool x = await RedisProvider.SetValue(RedisProvider.getConnectionAsync().GetAwaiter().GetResult().conn, 0, msg.pk, JsonConvert.SerializeObject(model));

                if (x)
                {
                    res = new ResponseDomain
                    {
                        Body = "",
                        HttpStatus = HttpStatusCode.Accepted
                    };
                }
                else
                {
                    res = new ResponseDomain
                    {
                        Body = "",
                        HttpStatus = HttpStatusCode.BadRequest
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
    }
}
