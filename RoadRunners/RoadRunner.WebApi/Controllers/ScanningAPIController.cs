using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoadRunners.Models;
using System.Threading;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System.Text;

namespace RoadRunner.WebApi.Controllers
{
    public class ScanningAPIController : ApiController
    {
        static string eventHubName = "carscans";
        static string connectionString = "Endpoint=sb://roadrunners.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=873ogmx1EZ0t3JOYXi7k7An2f8NyqRNhln1bggkLXVA=";

        // GET: api/ScanningAPI
        public IEnumerable<CarScan> Get()
        {
            CarScan cs = new CarScan()
            {
                Action = "Start",
                ScannerId = "A",
                LicensePlace = "1-ABC-AA"
            };
            return new CarScan[] { cs };
        }

        // GET: api/ScanningAPI/5
        public string Get(int id)
        {
            return "";
        }

        // POST: api/ScanningAPI
        public void Post([FromBody]CarScan scan)
        {
            //Sent the scan to event hub
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            var jsonString = JsonConvert.SerializeObject(scan);
            eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(jsonString)));
        }

        // PUT: api/ScanningAPI/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ScanningAPI/5
        public void Delete(int id)
        {
        }
    }
}
