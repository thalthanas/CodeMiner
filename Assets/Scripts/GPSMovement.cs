using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class GPSMovement : MonoBehaviour
{
    // Define a struct to hold GPS coordinates
    public struct GPSPoint
    {
        public double Latitude;
        public double Longitude;

        public GPSPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    // Current and previous GPS points
    private GPSPoint point1;
    private GPSPoint point2;

    // Flag to check if the first GPS point is set
    private bool isFirstPointSet = false;

    // Reference to the object to rotate in Unity
    public Transform objectToRotate;

    // Store the initial direction vector
    private Vector3 initialDirection = Vector3.zero;

    // Flag to check if the initial direction is set
    private bool isInitialDirectionSet = false;

    // Earth's radius in meters
    private const double EarthRadius = 6371000;

    public double f1, f2;


    // Boolean to control snapping behavior
    public bool snapRotation = false;

    void Start()
    {
        // Initialize with dummy data (replace with actual GPS data)
        //point1 = new GPSPoint(39.780847, 30.508062); // Initial GPS point (e.g., Los Angeles)
        //point2 = new GPSPoint(39.780847, 30.508062); // Same as point1 initially
    }

    IEnumerator waitAndMakeMovement()
    {
        // Simulate GPS updates (replace with actual GPS data retrieval)
        if (!isFirstPointSet)
        {
            // Set the first GPS point
            point1 = GetGPSData();
            isFirstPointSet = true;
        }
        else
        {
            // Get the second GPS point
            point2 = GetGPSData();

            // Calculate the vector between the two GPS points
            Vector3 gpsVector = CalculateGPSVector(point1, point2);

            if (gpsVector != Vector3.zero)
            {

                // If the initial direction is not set, set it to the first GPS vector
                if (!isInitialDirectionSet)
                {
                    initialDirection = gpsVector.normalized;
                    isInitialDirectionSet = true;
                }


                // Calculate the relative direction based on the initial direction
                Vector3 relativeDirection = CalculateRelativeDirection(gpsVector);

                


                // Calculate the distance between the two points
                float distance = (float)CalculateDistance(point1, point2);


                // Map the GPS vector to Unity's forward direction (Z-axis)
                //Quaternion targetRotation = Quaternion.LookRotation(gpsVector.normalized, Vector3.up);

                Quaternion targetRotation = Quaternion.LookRotation(relativeDirection, Vector3.up);


                // If snapping is enabled, snap to the closest axis
                if (snapRotation)
                {
                    // Get the target Y rotation in degrees
                    float targetYRotation = targetRotation.eulerAngles.y;

                    // Calculate the closest snapping angle (0°, 90°, 180°, 270°)
                    float snappedYRotation = SnapToClosestAxis(targetYRotation);

                    // Create a new rotation with the snapped Y angle
                    targetRotation = Quaternion.Euler(0, snappedYRotation, 0);

                    /*if (GetComponent<ClosestPointFinder>().distance <= 2.5f)
                    {
                        GetComponent<ClosestPointFinder>().snapRoad();
                    }*/
                }

                

                if(distance>=1)
                {
                    // Apply the rotation to the object
                    //objectToRotate.rotation = targetRotation;
                    objectToRotate.transform.DORotateQuaternion(targetRotation, 0.5f);

                    yield return new WaitForSeconds(0.501f);
                    // Move the object forward along its Z-axis by the calculated distance
                    //objectToRotate.Translate(Vector3.forward * distance, Space.Self);
                    // Smoothly move the object forward using DOTween
                    objectToRotate.DOMove(objectToRotate.position + objectToRotate.forward * distance, 0.5f); // Move over 1 second

                    // Update point1 to point2 for the next calculation
                    point1 = point2;
                }
               
            }
        }
    }

    // Calculate the relative direction based on the initial direction
    private Vector3 CalculateRelativeDirection(Vector3 gpsVector)
    {
        // Normalize the GPS vector
        Vector3 currentDirection = gpsVector.normalized;

        // Calculate the angle between the initial direction and the current direction
        float angle = Vector3.SignedAngle(initialDirection, currentDirection, Vector3.up);

        // Create a rotation based on the angle
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        // Rotate Unity's forward vector by the calculated angle
        Vector3 relativeDirection = rotation * Vector3.forward;

        return relativeDirection;
    }

    public void makeMovement()
    {
        StartCoroutine(waitAndMakeMovement());
    }

    // Helper method to snap the rotation to the closest axis
    private float SnapToClosestAxis(float targetYRotation)
    {
        // Define the snapping angles
        float[] snapAngles = { 0f, 90f, 180f, 270f };

        // Find the closest snapping angle
        float closestAngle = snapAngles[0];
        float smallestDifference = Mathf.Abs(Mathf.DeltaAngle(targetYRotation, snapAngles[0]));

        for (int i = 1; i < snapAngles.Length; i++)
        {
            float difference = Mathf.Abs(Mathf.DeltaAngle(targetYRotation, snapAngles[i]));
            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                closestAngle = snapAngles[i];
            }
        }

        return closestAngle;
    }


    // Simulate GPS data retrieval (replace with actual GPS input)
    private GPSPoint GetGPSData()
    {

        // Replace this with actual GPS data retrieval logic
        // For testing, simulate a small movement northward
        //return new GPSPoint(point2.Latitude + 0.0001, point2.Longitude);
        return new GPSPoint(f1, f2);
    }

    // Calculate the vector between two GPS points in Unity's 3D space
    private Vector3 CalculateGPSVector(GPSPoint p1, GPSPoint p2)
    {
        // Calculate the distance in meters
        double distance = CalculateDistance(p1, p2);

        // Calculate the direction vector in latitude and longitude
        double latDiff = p2.Latitude - p1.Latitude;
        double lonDiff = p2.Longitude - p1.Longitude;

        // Map the latitude and longitude differences to Unity's X and Z axes
        // Latitude corresponds to the Z-axis (forward/backward)
        // Longitude corresponds to the X-axis (left/right)
        Vector3 direction = new Vector3((float)lonDiff, 0, (float)latDiff).normalized;

        // Scale the direction vector by the distance
        return direction * (float)distance;
    }

    // Haversine formula to calculate the distance between two GPS points in meters
    private double CalculateDistance(GPSPoint p1, GPSPoint p2)
    {
        // Convert latitude and longitude from degrees to radians
        double lat1 = DegreesToRadians(p1.Latitude);
        double lon1 = DegreesToRadians(p1.Longitude);
        double lat2 = DegreesToRadians(p2.Latitude);
        double lon2 = DegreesToRadians(p2.Longitude);

        // Difference in coordinates
        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;

        // Haversine formula
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Calculate the distance in meters
        return EarthRadius * c;
    }

    // Helper method to convert degrees to radians
    private double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}