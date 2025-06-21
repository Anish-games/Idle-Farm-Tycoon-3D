using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera targetCamera; 

    void LateUpdate()
    {
        
        Vector3 targetPosition = transform.position + targetCamera.transform.rotation * Vector3.forward;
       
        Vector3 upDirection = targetCamera.transform.rotation * Vector3.up;

        
        transform.LookAt(targetPosition, upDirection);
    }
}