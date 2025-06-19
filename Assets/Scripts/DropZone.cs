using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    [Header("Drop Zone Settings")]
    public int capacity = 4;
    public Transform dropPoint;
    public float dropOffsetY = 0.6f;

    [Header("Processing")]
    public ProcessingFactory processingFactory;

    private List<Transform> collectedItems = new List<Transform>();
    private bool playerInZone = false;
    private bool isProcessing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isProcessing)
        {
            playerInZone = true;
            StackCollector collector = other.GetComponentInChildren<StackCollector>();

            if (collector != null)
            {
                int allowedToDrop = Mathf.Min(capacity - collectedItems.Count, collector.GetStackCount());

                if (allowedToDrop > 0)
                {
                    collector.StartUnloadLimited(
                        dropPoint,
                        dropOffsetY,
                        () => playerInZone,
                        allowedToDrop,
                        OnItemDropped
                    );
                }
                else
                {
                    Debug.Log("Drop zone is full!");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInZone = false;
    }

    private void OnItemDropped(Transform item)
    {
        CollectibleType type = item.GetComponent<CollectibleType>();
        if (type != null && type.type != CollectibleKind.Old)
        {
            Debug.Log("Only old collectibles can be dropped here.");
            return;
        }

        collectedItems.Add(item);

        if (collectedItems.Count >= capacity && !isProcessing && processingFactory != null && processingFactory.CanProcess())
        {
            Debug.Log("Drop zone full, sending to factory.");
            isProcessing = true;

            processingFactory.StartProcessing(new List<Transform>(collectedItems), () =>
            {
                collectedItems.Clear();
                isProcessing = false;
                Debug.Log("Drop zone cleared, ready for next input.");
            });
        }
    }



}
