namespace RoadRunners.CarActor
{
    using Microsoft.ServiceFabric.Actors.Runtime;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Threading.Tasks;
    using Interfaces;
    using Models;

    [StatePersistence(StatePersistence.Persisted)]
    internal class CarActor : Actor, ICarActor
    {
        //protected override Task OnActivateAsync()
        //{
        //    ActorEventSource.Current.ActorMessage(this, "Actor activated.");
        //    return this.StateManager.TryAddStateAsync("state", CarStates.Unknown);
        //}

        async Task<CarStates> ICarActor.GetStateAsync()
        {
            var scan = await this.StateManager.GetStateAsync<CarScan>("state");

            return scan.Action;
        }

        static Task<double> GetDistanceAsync(string startScannerId, string endScannerId)
        {
            return Task.FromResult(4.0); // cheating...
        }

        static async Task<double> ComputeSpeedAsync(CarScan carScan)
        {
            var distanceInKm = await GetDistanceAsync(carScan.StartScannerId, carScan.EndScannerId);

            var durationInHours = carScan.EndTime.Subtract(carScan.StartTime).TotalHours;

            var speed = distanceInKm / durationInHours;

            return speed;
        }

        async Task ICarActor.SetStateAsync(CarScan carscan)
        {
            CarScan cs;
            var storedCarScan = await this.StateManager.TryGetStateAsync<CarScan>("state");
            if (!storedCarScan.HasValue)
                cs = carscan;
            else
                cs = storedCarScan.Value;

            switch (carscan.Action)
            {
                case CarStates.Start:
                    break;
                case CarStates.End:
                    cs.EndScannerId = carscan.EndScannerId;
                    cs.EndTime = carscan.EndTime;
                    cs.Speed = await ComputeSpeedAsync(carscan);
                    break;

            }
            // Requests are not guaranteed to be processed in order nor at most once.
            // The update function here verifies that the incoming state is greater than the current count to preserve order.
            if (carscan.Action == CarStates.End)
                StoreEvent(carscan);
            await this.StateManager.AddOrUpdateStateAsync("state", cs, (key, value) => cs.Action > value.Action ? cs : value);
        }

        private void StoreEvent(CarScan carscan)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=mindparkwebjobs;AccountKey=D6BZdyaZq+kGMxq5H1oAXy9rMElxmcQFMy581uh8lbtDrt3cDEiiOzLoogQu8y6uO2WWixXQbVojURIJKCi3TA==";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("carscans");
            table.CreateIfNotExists();

            CarScanStorable carscanStorable = new CarScanStorable(carscan);
            TableOperation insertOperation = TableOperation.Insert(carscanStorable);
            table.Execute(insertOperation);
        }
    }
}
