using SK.GeolocatorWebGL.Models;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SK.GeolocatorWebGL.Examples
{
    public class GetLocationButton : MonoBehaviour
    {
        public Button Button;
        public GeolocationText Text;

        public GameObject label;
        bool isLabelOpened;

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

                SK_Geolocator.GetCurrentLocation((s) => Text.UpdateText(s), (e) => Text.UpdateText(e), options);
                SK_Geolocator.WatchLocation((s) => Text.UpdateText(s), (e) => Text.UpdateText(e));
            });
        }


        public void openLabel()
        {
            if(isLabelOpened == false)
            {
                StartCoroutine(waitAndDisableLabel());
                isLabelOpened = true;
            }
            
        }
        IEnumerator waitAndDisableLabel()
        {
            label.SetActive(true);
            yield return new WaitForSeconds(5);
            label.SetActive(false);
            Text.isAccurate = true;
        }
    }
}
