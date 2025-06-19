using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cameraController : MonoBehaviour
{
    private Transform target; // Reference to the player
    private Vector3 velocity = Vector3.zero; // For SmoothDamp

    // Desired Camera Offset (Adjust as needed)
    private Vector3 offset = new Vector3(0f, 15f, -10.02f);

    void Start()
    {
        // Auto-find the player if not assigned
        target = FindObjectOfType<PlayerController>()?.transform;

        // Ensure initial position is set correctly
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            // Smoothly follow the player while maintaining offset
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
        }
    }


}