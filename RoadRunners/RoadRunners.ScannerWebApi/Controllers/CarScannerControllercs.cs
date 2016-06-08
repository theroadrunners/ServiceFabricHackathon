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

namespace RoadRunners.ScannerWebApi.Controllers
{
    class CarScannerControllercs
    {

        private static Uri serviceUri = new Uri("fabric:/RoadRunners.SFCluster/RoadRunners.CarActor");

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

        // POST api/values 
        public void Post([FromBody]CarScan carscan)
        {
            ActorId actorId = new ActorId(carscan.LicensePlace);
            ICarActor carActor = ActorProxy.Create<ICarActor>(actorId, serviceUri);
            switch (carscan.Action)
            {
                case "Start":
                    carActor.SetStateAsync(CarStates.Start);
                    break;
                case "End":
                    carActor.SetStateAsync(CarStates.End);
                    break;
            }
        }
    }
}
