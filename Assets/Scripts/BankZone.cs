using UnityEngine;

public class BankZone : MonoBehaviour
{
    public Transform dropPoint;
    public float dropOffsetY = 0.6f;
    public int coinsPerCollectible = 10;

    private bool playerInZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInZone = true;

        //  Grab ALL collectors on the player:
        var collectors = other.GetComponentsInChildren<StackCollector>();
        StackCollector newCollector = null;

        
        foreach (var col in collectors)
        {
            if (col.GetStackCount(CollectibleKind.New) > 0)
            {
                newCollector = col;
                break;
            }
        }

        if (newCollector == null)
        {
           
            return;
        }

        int newCount = newCollector.GetStackCount(CollectibleKind.New);
        

        
        newCollector.StartUnloadLimited(
            dropPoint,
            dropOffsetY,
            () => playerInZone,
            newCount,
            OnNewDropped
        );
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInZone = false;
    }

    private void OnNewDropped(Transform item)
    {
        item.GetComponent<PoolableObject>()?.ReturnToPool();
        CoinManager.Instance.CollectCoin(item.position);
    }
}
