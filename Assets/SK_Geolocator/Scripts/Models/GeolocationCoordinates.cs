using System;

namespace SK.GeolocatorWebGL.Models
{
    [Serializable]
    public class GeolocationCoordinates
    {
        public double accuracy;
        public double? altitude;
        public double? altitudeAccuracy;
        public double? heading;
        public double latitude;
        public double longitude;
        public double speed;
    }
}

