using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadRunners.CarActor.Interfaces;
using RoadRunners.Models;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors;
using Newtonsoft.Json;

namespace RoadRunners.ScannerWebApi.Controllers
{
    public class ScannerController : ApiController
    {
        private static Uri serviceUri = new Uri("fabric:/RoadRunners.SFCluster/CarActorService");

        public IEnumerable<CarScan> Get()
        {
            CarScan cs = new CarScan()
            {
                Action = CarStates.Start,
                ScannerId = "A",
                LicensePlate = "1-ABC-AA"
            };
            return new CarScan[] { cs };
        }

        public string Get(int id)
        {
            return "value";
        }

        public async Task Post([FromBody]string carscanStr)
        {
            var carscan = JsonConvert.DeserializeObject<CarScan>(carscanStr);
            if (carscan.Action == CarStates.Start)
                carscan.StartTime = DateTime.Now;
            if (carscan.Action == CarStates.End)
                carscan.EndTime = DateTime.Now;
            ActorId actorId = new ActorId(carscan.LicensePlate);
            ICarActor carActor = ActorProxy.Create<ICarActor>(actorId, serviceUri);
            await carActor.SetStateAsync(carscan);

            return;
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
