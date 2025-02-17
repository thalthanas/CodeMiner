using System;
using SK.GeolocatorWebGL.Models;
using UnityEngine;

namespace SK.GeolocatorWebGL
{
    public static class SK_Geolocator
    {
        public static int LastWatchId { get; private set; }

        private static bool _initialized;
        private static Action<GeolocationPosition> _onLocation;
        private static Action<GeolocationPositionError> _onLocationError;
        private static Action<GeolocationPosition> _onWatch;
        private static Action<GeolocationPositionError> _onWatchError;

        private static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            SK_GeolocatorJsLib.Init("SK_Geolocator");
        }

        public static bool IsAvailable()
        {
            return SK_GeolocatorJsLib.IsAvailable();
        }

        public static void GetCurrentLocation(Action<GeolocationPosition> onLocation, Action<GeolocationPositionError> onError, PositionOptions options = null)
        {
            Initialize();
            _onLocation = onLocation;
            _onLocationError = onError;

            SK_GeolocatorJsLib.OnCurrentLocationEvent.RemoveAllListeners();
            SK_GeolocatorJsLib.OnCurrentLocationErrorEvent.RemoveAllListeners();

            SK_GeolocatorJsLib.OnCurrentLocationEvent.AddListener(OnCurrentPosition);
            SK_GeolocatorJsLib.OnCurrentLocationErrorEvent.AddListener(OnCurrentPositionError);

            SK_GeolocatorJsLib.GetCurrentPosition(JsonUtility.ToJson(options ?? new PositionOptions()));
        }

        public static int WatchLocation(Action<GeolocationPosition> onWatch, Action<GeolocationPositionError> onError, PositionOptions options = null)
        {
            Initialize();
            _onWatch = onWatch;
            _onWatchError = onError;

            SK_GeolocatorJsLib.OnWatchPositionEvent.RemoveAllListeners();
            SK_GeolocatorJsLib.OnWatchErrorEvent.RemoveAllListeners();

            SK_GeolocatorJsLib.OnWatchPositionEvent.AddListener(OnWatchLocation);
            SK_GeolocatorJsLib.OnWatchErrorEvent.AddListener(OnWatchError);

            LastWatchId = SK_GeolocatorJsLib.Watch(JsonUtility.ToJson(options ?? new PositionOptions()));
            return LastWatchId;
        }

        public static void ClearWatch(int? watchId = null)
        {
            Initialize();
            if (!watchId.HasValue)
            {
                _onWatch = null;
                _onWatchError = null;
                watchId = LastWatchId;
            }

            SK_GeolocatorJsLib.ClearWatch(watchId.Value);
        }

        private static void OnWatchLocation(string payload, byte[] _)
        {
            Debug.Log(payload);
            var position = JsonUtility.FromJson<GeolocationPosition>(payload);
            _onWatch?.Invoke(position);
        }

        private static void OnWatchError(string payload, byte[] _)
        {
            Debug.Log(payload);
            var error = JsonUtility.FromJson<GeolocationPositionError>(payload);
            _onWatchError?.Invoke(error);
        }

        private static void OnCurrentPosition(string payload, byte[] _)
        {
            Debug.Log(payload);
            var position = JsonUtility.FromJson<GeolocationPosition>(payload);
            _onLocation?.Invoke(position);
        }

        private static void OnCurrentPositionError(string payload, byte[] _)
        {
            Debug.Log(payload);
            var position = JsonUtility.FromJson<GeolocationPositionError>(payload);
            _onLocationError?.Invoke(position);
        }
    }
}