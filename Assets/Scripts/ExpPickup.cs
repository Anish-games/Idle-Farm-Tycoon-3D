using UnityEngine;

public class XpPickup : MonoBehaviour
{
    public float pickupRange = 5f;
    public float moveSpeed = 5f;

    private bool isCollected = false;
    private Transform player;
    private StackCollector stackCollector;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            stackCollector = player.GetComponentInChildren<StackCollector>(); // StackCollector must be on a child
        }
    }

    void Update()
    {
        if (isCollected || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < pickupRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected || other.CompareTag("Player") == false) return;

        if (stackCollector != null)
        {
            isCollected = true;

            // Disable movement physics
            if (TryGetComponent<Rigidbody>(out var rb))
                rb.isKinematic = true;

            GetComponent<Collider>().enabled = false; // Optional: disable collision after collect

            stackCollector.StackItem(transform);
        }
    }
}
