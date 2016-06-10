namespace RoadRunners.Models
{
    using System;

    public class CarScan
    {
        public string LicensePlate { get; set; }
        public string StartScannerId { get; set; }
        public string EndScannerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Speed { get; set; }
        public CarStates Action { get; set; }
    }
}