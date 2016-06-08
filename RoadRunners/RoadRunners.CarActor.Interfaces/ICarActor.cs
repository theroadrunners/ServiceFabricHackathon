namespace RoadRunners.CarActor.Interfaces
{
    using Microsoft.ServiceFabric.Actors;
    using System.Threading.Tasks;
    using Models;

    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface ICarActor : IActor
    {
        /// <summary>
        /// TODO: Replace with your own actor method.
        /// </summary>
        /// <returns></returns>
        Task<CarStates> GetStateAsync();

        /// <summary>
        /// TODO: Replace with your own actor method.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task SetStateAsync(CarScan carscan);
    }
}
