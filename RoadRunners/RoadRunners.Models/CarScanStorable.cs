namespace RoadRunners.Models
{
    using System;
    using Microsoft.WindowsAzure.Storage.Table;

    public enum CarStates
    {
        Unknown = 0,
        Start = 1,
        End = 2
    }

    [Serializable]
    public class CarScanStorable : TableEntity
    {
        public CarScanStorable(CarScan carScan)
        {
            this.PartitionKey = "A";
            this.RowKey = $"{carScan.LicensePlate}";
            this.LicensePlate = carScan.LicensePlate;
            this.StartScannerId = carScan.StartScannerId;
            this.EndScannerId = carScan.EndScannerId;
            this.StartTime = carScan.StartTime;
            this.EndTime = carScan.EndTime;
            this.Action = (int)carScan.Action;
            this.Speed = carScan.Speed;
        }

        public string LicensePlate { get; set; }

        /// <summary>
        /// The device that scans cars on the road
        /// </summary>
        public string StartScannerId { get; set; }

        /// <summary>
        /// The device that scans cars on the road
        /// </summary>

        public string EndScannerId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public double Speed { get; set; }

        /// <summary>
        /// Can be Start of End
        /// </summary>
        public int Action { get; set; }
    }
}
