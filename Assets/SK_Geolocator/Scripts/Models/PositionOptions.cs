using System;

namespace SK.GeolocatorWebGL.Models
{
    [Serializable]
    public class PositionOptions
    {
        public bool enableHighAccuracy;
        public double maximumAge;
        public double timeout;
    }
}

