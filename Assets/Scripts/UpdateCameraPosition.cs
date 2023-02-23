using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCameraPosition : MonoBehaviour
{
    public Transform player1;  // Transform component of player 1
    public Transform player2;  // Transform component of player 2

    private Vector3 cameraPosition;
    private float currentFov;  // Current FOV of the camera
    private float desiredFov;  // Desired FOV of the camera

    private void Update()
    {
        // Determine the center position and distance between the players
        Vector3 center = (player1.position + player2.position) / 2;
        float distance = Vector3.Distance(player1.position, player2.position);

        // Adjust the camera's FOV based on the distance between the players
        float minFov = 80;  // Minimum FOV value
        float maxFov = 140;  // Maximum FOV value
        float minDistance = 20;  // Minimum distance at which to start zooming in
        float maxDistance = 5;  // Maximum distance at which to start zooming out
        desiredFov = Mathf.Lerp(maxFov, minFov, (distance - minDistance) / (maxDistance - minDistance));
        
        // Interpolate the camera's FOV towards the desired value
        float lerpFactor = 0.01f;  // Controls the smoothness of the camera zoom
        currentFov = Mathf.Lerp(currentFov, desiredFov, lerpFactor);
        
        // Offset the camera position to center on the player
        Vector3 desiredCameraPos = center - transform.forward * 5;

        // Interpolate the camera position towards the desired position
        lerpFactor = 0.01f;  // Controls the smoothness of the camera panning
        cameraPosition = Vector3.Lerp(transform.position, desiredCameraPos, lerpFactor);

        // Clamp the camera position to the bounds of the level
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -10, 10);  // Level width value
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, -10, 10);  // Level height value

        // Set the camera's FOV and position
        GetComponent<Camera>().fieldOfView = currentFov;
        transform.position = cameraPosition;
    }
}


