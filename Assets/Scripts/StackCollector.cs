using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCollector : MonoBehaviour
{
    private List<Transform> collectedItems = new List<Transform>();

    public Transform stackHolder;
    public float verticalOffset = 2f;
    public float SlowTheTimeBy = 0.1f;

    [Header("Sound")]
    public AudioClip collectSound;
    public AudioClip dropSound;
    private AudioSource audioSource;

    private int stackCount = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StackItem(Transform item)
    {
        collectedItems.Add(item); // Add new item to bottom of stack (end of list)
        stackCount++;

        item.SetParent(stackHolder);
        item.localRotation = Quaternion.identity;
        item.localScale = Vector3.one;

        RebuildStackVisuals();

        if (collectSound && audioSource)
            audioSource.PlayOneShot(collectSound);
    }

    private void RebuildStackVisuals()
    {
        // Reposition items so index 0 is on top, last is at bottom
        for (int i = 0; i < collectedItems.Count; i++)
        {
            int reverseIndex = collectedItems.Count - 1 - i; // So first collected is at top
            collectedItems[i].localPosition = new Vector3(0, reverseIndex * verticalOffset, 0);
        }
    }

    public int GetStackCount()
    {
        return stackCount;
    }

    public void StartUnloadLimited(Transform dropPoint, float offsetY, Func<bool> isPlayerStillInZone, int limit, Action<Transform> onItemDropped)
    {
        if (collectedItems.Count > 0)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateLimitedUnload(dropPoint, offsetY, isPlayerStillInZone, limit, onItemDropped));
        }
    }

    private IEnumerator AnimateLimitedUnload(Transform dropPoint, float offsetY, Func<bool> isPlayerStillInZone, int limit, Action<Transform> onItemDropped)
    {
        int itemsToDrop = Mathf.Min(limit, collectedItems.Count);

        for (int i = 0; i < itemsToDrop; i++) // Drop from top down
        {
            if (!isPlayerStillInZone()) yield break;

            Transform item = collectedItems[i];
            item.SetParent(null);

            Vector3 targetPos = dropPoint.position + new Vector3(0, i * offsetY, 0);
            float duration = 0.3f;
            float timePassed = 0f;
            Vector3 startPos = item.position;

            while (timePassed < duration)
            {
                if (!isPlayerStillInZone()) yield break;

                item.position = Vector3.Lerp(startPos, targetPos, timePassed / duration);
                timePassed += Time.deltaTime;
                yield return null;
            }

            item.position = targetPos;

            if (dropSound && audioSource)
                audioSource.PlayOneShot(dropSound);

            onItemDropped?.Invoke(item);
            yield return new WaitForSeconds(SlowTheTimeBy);
        }

        // Remove dropped items from top of the list
        collectedItems.RemoveRange(0, itemsToDrop);
        stackCount -= itemsToDrop;

        RebuildStackVisuals(); // Re-stack remaining items
    }
}
