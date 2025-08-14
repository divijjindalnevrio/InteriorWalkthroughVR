using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [Header("Look At Settings")]
    [SerializeField] private Transform targetCamera; // Reference to the camera
    [SerializeField] private bool lockYAxis = true; // Whether to lock rotation to Y-axis only
    [SerializeField] private float rotationSpeed = 5f; // Speed of rotation (optional)
    
    private Transform cameraTransform;
    
    void Start()
    {
        // If no camera is assigned, try to find the main camera
        if (targetCamera == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
            }
            else
            {
                Debug.LogWarning("No main camera found! Please assign a camera in the inspector.");
            }
        }
        else
        {
            cameraTransform = targetCamera;
        }
    }
    
    void Update()
    {
        if (cameraTransform != null)
        {
            LookAtCameraY();
        }
    }
    
    void LookAtCameraY()
    {
        // Get the direction from this object to the camera
        Vector3 directionToCamera = cameraTransform.position - transform.position;
        
        if (lockYAxis)
        {
            // Zero out the Y component to only rotate on Y-axis
            directionToCamera.y = 0;
        }
        
        // Only rotate if there's a valid direction
        if (directionToCamera != Vector3.zero)
        {
            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            
            // Apply the rotation
            if (rotationSpeed > 0)
            {
                // Smooth rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // Instant rotation
                transform.rotation = targetRotation;
            }
        }
}
}