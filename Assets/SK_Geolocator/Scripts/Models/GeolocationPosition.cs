using System;

namespace SK.GeolocatorWebGL.Models
{
    [Serializable]
    public class GeolocationPosition
    {
        public int watchId;
        public GeolocationCoordinates coords;
        public long timestamp;
    }
}

