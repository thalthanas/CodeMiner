using UnityEngine;
using UnityEngine.UI;

namespace SK.GeolocatorWebGL.Examples
{
    public class ClearWatchButton : MonoBehaviour
    {
        public Button Button;
        public GeolocationText Text;

        private void Awake()
        {
            Button.onClick.AddListener(() =>
            {
                SK_Geolocator.ClearWatch();
            });
        }
    }
}
