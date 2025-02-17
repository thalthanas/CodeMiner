using UnityEngine;
using UnityEngine.UI;
using SK.GeolocatorWebGL.Models;

namespace SK.GeolocatorWebGL.Examples
{
    public class WatchLocationButton : MonoBehaviour
    {
        public Button Button;
        public GeolocationText Text;

        private void Awake()
        {
            Button.onClick.AddListener(() =>
            {
                var options = new PositionOptions
                {
                    enableHighAccuracy = true,
                    maximumAge = 100,
                    timeout = 10_000
                };
                SK_Geolocator.WatchLocation((s) => Text.UpdateText(s), (e) => Text.UpdateText(e));
            });
        }
    }
}
