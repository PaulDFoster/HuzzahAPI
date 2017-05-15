using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using System.Threading.Tasks;

namespace HuzzahAPI.Controllers
{
    public class StockController : ApiController
    {
        static ServiceClient serviceClient;
        static string connectionString = "HostName=paulfotest.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=V2PR5Yjtl1Bzc6DhbDKuopaRtOkhA1pbeOc61q9yyq8=";

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public async Task Post([FromBody]string value)
        {
            if (value.Length == 0) return; // should return a failure
            
            //try to parse the supplied JSON string
            try
            {
                DeviceMethod deviceMethod = JsonConvert.DeserializeObject<DeviceMethod>(value);
                // ToDo: Validate device is correct and available
                // ToDo: Validate caller is allowed to contact the device
                // ToDo: Validate device has the required flavours?? Or device can meet request?
                // Make the call
                string responseJSON = await CallDeviceMethod(deviceMethod.deviceId, deviceMethod.method, deviceMethod.payload);
                // ToDo: Evaluate response JSON and return appropriate response to caller
            }
            catch
            {
                // ToDo: Return a failure response to the caller
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        private static async Task<string> CallDeviceMethod(string deviceId, string method, string parameters)
        {
            var methodInvocation = new CloudToDeviceMethod(method) { ResponseTimeout = TimeSpan.FromSeconds(30) };

            methodInvocation.SetPayloadJson(parameters);

            var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, methodInvocation);

            return response.GetPayloadAsJson();
        }

        private class DeviceMethod
        {
            public string deviceId;
            public string method;
            public string payload;
        }

    }


}