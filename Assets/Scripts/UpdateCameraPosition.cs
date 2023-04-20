using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCameraPosition : MonoBehaviour
{
    public Transform player1;  // Transform component of player 1
    public Transform player2;  // Transform component of player 2

    private Vector3 cameraPosition;
    private float currentSize;  // Current size of the camera
    private float desiredSize;  // Desired size of the camera

    private void Update()
    {
        // Determine the center position and distance between the players
        Vector3 center = (player1.position + player2.position) / 2;
        float distance = Vector3.Distance(player1.position, player2.position);

        // Adjust the camera's size based on the distance between the players
        float minSize = 4;  // Minimum size value
        float maxSize = 12;  // Maximum size value
        float minDistance = 30;  // Minimum distance at which to start zooming in
        float maxDistance = 10;  // Maximum distance at which to start zooming out
        desiredSize = Mathf.Lerp(minSize, maxSize, (distance - maxDistance) / (minDistance - maxDistance));
        
        // Interpolate the camera's size towards the desired value
        float lerpFactor = 0.005f;  // Controls the smoothness of the camera zoom
        currentSize = Mathf.Lerp(currentSize, desiredSize, lerpFactor);
        
        // Offset the camera position to center on the player
        Vector3 desiredCameraPos = center - transform.forward * 5;

        // Interpolate the camera position towards the desired position
        lerpFactor = 0.01f;  // Controls the smoothness of the camera panning
        cameraPosition = Vector3.Lerp(transform.position, desiredCameraPos, lerpFactor);

        // Clamp the camera position to the bounds of the level
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -10, 10);  // Level width value
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, -10, 10);  // Level height value

        // Set the camera's size and position
        GetComponent<Camera>().orthographicSize = currentSize;
        transform.position = cameraPosition;
    }
}

