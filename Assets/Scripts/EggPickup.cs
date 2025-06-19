using UnityEngine;

public class EggPickup : MonoBehaviour
{
    public float pickupRange = 5f;
    public float moveSpeed = 5f;

    private bool isCollected = false;
    private Transform player;
    private StackCollector oldCollector;
    private StackCollector newCollector;
    private CollectibleType collectibleType;

    void Start()
    {
        collectibleType = GetComponent<CollectibleType>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;

            StackCollector[] collectors = player.GetComponentsInChildren<StackCollector>();
            foreach (var collector in collectors)
            {
                if (collector.gameObject.name.Contains("New"))
                    newCollector = collector;
                else
                    oldCollector = collector;
            }
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
        if (isCollected || !other.CompareTag("Player")) return;

        StackCollector targetCollector = null;

        if (collectibleType != null)
        {
            if (collectibleType.type == CollectibleKind.New)
                targetCollector = newCollector;
            else
                targetCollector = oldCollector;
        }

        if (targetCollector != null)
        {
            isCollected = true;

            if (TryGetComponent<Rigidbody>(out var rb))
                rb.isKinematic = true;

            GetComponent<Collider>().enabled = false;

            targetCollector.StackItem(transform);
        }
    }
}
