using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRunners.Models
{
    public enum CarStates
    {
        Unknown = 0,
        Start = 1,
        End = 2
    }

    [Serializable]
    public class CarScan
    {
        private string _licensePlate;

        /// <summary>
        /// Licenseplate of the car
        /// </summary>
        public string LicensePlate
        {
            get { return _licensePlate; }
            set { _licensePlate = value; }
        }

        private String _startScannerId;

        /// <summary>
        /// The device that scans cars on the road
        /// </summary>
        public string StartScannerId
        {
            get { return _startScannerId; }
            set { _startScannerId = value; }
        }

        private String _endScannerId;

        /// <summary>
        /// The device that scans cars on the road
        /// </summary>
        public string EndScannerId
        {
            get { return _endScannerId; }
            set { _endScannerId = value; }
        }

        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        private double _speed = 0;

        public double Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private CarStates _action;

        /// <summary>
        /// Can be Start of End
        /// </summary>
        public CarStates Action
        {
            get { return _action; }
            set { _action = value; }
        }

    }
}
