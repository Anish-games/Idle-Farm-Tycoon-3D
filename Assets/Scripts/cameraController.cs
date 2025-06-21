using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    private Vector3 velocity = Vector3.zero;
    private Vector3 offset = new Vector3(0f, 15f, -10.02f);

    void Start()
    {
        target = FindObjectOfType<PlayerController>()?.transform;
        if (target) transform.position = target.position + offset;
    }

    void FixedUpdate()
    {
        if (target)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
        }
    }
}
