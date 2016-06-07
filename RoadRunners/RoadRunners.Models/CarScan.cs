using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadRunners.Models
{
    public class CarScan
    {
        private string _licensePlate;

        /// <summary>
        /// Licenseplate of the car
        /// </summary>
        public string LicensePlace
        {
            get { return _licensePlate; }
            set { _licensePlate = value; }
        }

        private String _scannerId;

        /// <summary>
        /// The device that scans cars on the road
        /// </summary>
        public string ScannerId
        {
            get { return _scannerId; }
            set { _scannerId = value; }
        }

        public string _action;

        /// <summary>
        /// Can be Start of End
        /// </summary>
        public string Action
        {
            get { return _scannerId;  }
            set { _action = value;  }
        }



    }
}
