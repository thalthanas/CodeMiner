using System.Text;
using SK.GeolocatorWebGL.Models;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace SK.GeolocatorWebGL.Examples
{
    public class GeolocationText : MonoBehaviour
    {
        public Text Text;

        public GameObject myChar;

        public GameObject myController;

        public TextMeshProUGUI myLat, myLong, myAcc;

        float myTimer;

        public bool isAccurate;

        public float desiredAcc;

        public TMP_InputField desiredAccInputField;

        private void Start()
        {
            desiredAcc = 6;
        }

        private void Update()
        {
            myTimer += 1 * Time.deltaTime;
        }

        public void UpdateText(GeolocationPosition position)
        {
            
                var str = new StringBuilder();
                str.AppendLine($"Accuracy: {position.coords.accuracy}");
                str.AppendLine($"Altitude: {position.coords.altitude}");
                str.AppendLine($"Altitude Accuracy: {position.coords.altitudeAccuracy}");
                str.AppendLine($"Heading: {position.coords.heading}");
                str.AppendLine($"Latitude: {position.coords.latitude}");
                str.AppendLine($"Longitude: {position.coords.longitude}");

            Text.text = str.ToString();

            myLat.text = $"Latitude: {position.coords.latitude}";
            myLong.text = $"Longitude: {position.coords.longitude}";
            myAcc.text = $"Accuracy: {position.coords.accuracy}";

            if (myTimer >= 3 && isAccurate == true)
            {
                myTimer = 0;
                myController.GetComponent<GPSMovement>().f1 = position.coords.latitude;
                myController.GetComponent<GPSMovement>().f2 = position.coords.longitude;
                myController.GetComponent<GPSMovement>().makeMovement();
            }

            if(position.coords.accuracy<=desiredAcc)
            {
                myController.GetComponent<GPSMovement>().snapRotation = true;
            }
            else
            {
                myController.GetComponent<GPSMovement>().snapRotation = false;
            }

            


            
        }

        public void changeDesiredAcc()
        {
            
            desiredAcc = float.Parse(desiredAccInputField.text);
        }

        public void UpdateText(GeolocationPositionError error)
        {
            Text.text = error.message;
        }

        
    }
}