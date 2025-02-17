using System;

namespace SK.GeolocatorWebGL.Models
{
    [Serializable]
    public class GeolocationPositionError
    {
        public string code;
        public string message;
        public int PERMISSION_DENIED;
        public int POSITION_UNAVAILABLE;
        public int TIMEOUT;
    }
}

