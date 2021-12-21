using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace InformationService
{
    [DataContract]
    public class Telemetry
    {
        private string spacecraftName;
        private double altitude;
        private double latitude;
        private double longitude;
        private double timeToOrbit;
        private double temperature;
        private double counter;

        [DataMember]
        public string SpacecraftName { get { return spacecraftName; } set { spacecraftName = value; } }
        [DataMember]
        public double Altitude { get { return altitude; } set { altitude = value; } }
        [DataMember]
        public double Latitude { get { return latitude; } set { latitude = value; } }
        [DataMember]
        public double Longitude { get { return longitude; } set { longitude = value; } }
        [DataMember]
        public double TimeToOrbit { get { return timeToOrbit; } set { timeToOrbit = value; } }
        [DataMember]
        public double Temperature { get { return temperature; } set { temperature = value; } }
        [DataMember]
        public double Counter { get { return counter; } set { counter = value; } }
    }
}
