﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using RoadRunners.CarActor.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using RoadRunners.Models;

namespace RoadRunners.CarActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class CarActor : Actor, ICarActor
    {
        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see http://aka.ms/servicefabricactorsstateserialization

            return this.StateManager.TryAddStateAsync("state", new CarScan { Action = CarStates.Unknown });
        }

        /// <summary>
        /// TODO: Replace with your own actor method.
        /// </summary>
        /// <returns></returns>
        Task<CarStates> ICarActor.GetStateAsync()
        {
            return this.StateManager.GetStateAsync<CarStates>("state");
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

        /// <summary>
        /// TODO: Replace with your own actor method.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        async Task ICarActor.SetStateAsync(CarScan carscan)
        {
            try
            {
                CarScan cs;
                var storedCarScan = await this.StateManager.GetStateAsync<CarScan>("state");
                if (storedCarScan.Action == CarStates.Unknown)
                    cs = carscan;
                else
                    cs = storedCarScan;

                switch (carscan.Action)
                {
                    case CarStates.Start:
                        break;
                    case CarStates.End:
                        cs.EndScannerId = carscan.EndScannerId;
                        cs.EndTime = carscan.EndTime;
                        cs.Speed = await ComputeSpeedAsync(cs);
                        break;

                }
                // Requests are not guaranteed to be processed in order nor at most once.
                // The update function here verifies that the incoming state is greater than the current count to preserve order.
                if (carscan.Action == CarStates.End)
                    StoreEvent(cs);
                await this.StateManager.AddOrUpdateStateAsync("state", cs, (key, value) => cs.Action > value.Action ? cs : value);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private void StoreEvent(CarScan carscan)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=mindparkwebjobs;AccountKey=D6BZdyaZq+kGMxq5H1oAXy9rMElxmcQFMy581uh8lbtDrt3cDEiiOzLoogQu8y6uO2WWixXQbVojURIJKCi3TA==";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("CarScans");
            table.CreateIfNotExists();

            CarScanStorable carscanStorable = new CarScanStorable(carscan);
            
            TableOperation insertOperation = TableOperation.Insert(carscanStorable);
            table.Execute(insertOperation);
        }
    }
}
