using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    [Header("Drop Zone")]
    public int capacity = 4;
    public Transform dropPoint;
    public float dropOffsetY = 0.6f;

    [Header("Factory")]
    public ProcessingFactory processingFactory;
    public DropZoneUpgradeUI upgradeUI;

    private List<Transform> collectedItems = new();
    private bool playerInZone, isProcessing;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInZone = true;
        TryDropOld();
        upgradeUI?.ShowUI(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInZone = false;
        upgradeUI?.HideUI();
    }

    void OnItemDropped(Transform item)
    {
        if (item.GetComponent<CollectibleType>()?.type != CollectibleKind.Old)
            return;

        collectedItems.Add(item);

        if (collectedItems.Count >= capacity && !isProcessing && processingFactory.CanProcess())
        {
            isProcessing = true;
            processingFactory.StartProcessing(new List<Transform>(collectedItems), () =>
            {
                collectedItems.Clear();
                isProcessing = false;
                if (playerInZone) TryDropOld();
            });
        }
    }

    void TryDropOld()
    {
        if (isProcessing) return;

        var player = GameObject.FindGameObjectWithTag("Player");
        var col = player?.GetComponentInChildren<StackCollector>();
        if (col == null) return;

        // Drop only Old
        int haveOld = col.GetStackCount(CollectibleKind.Old);
        if (haveOld == 0) return;

        int allowed = Mathf.Min(capacity - collectedItems.Count, haveOld);
        if (allowed > 0)
        {
            col.StartUnloadLimited(dropPoint, dropOffsetY,
                () => playerInZone, allowed, OnItemDropped);
        }
    }
}