using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackCollector : MonoBehaviour
{
    public Transform stackHolder;
    public float verticalOffset = 2f;
    public float slowTimeBetweenDrops = 0.1f;

    public AudioClip collectSound;
    public AudioClip dropSound;

    private List<Transform> collectedItems = new();
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StackItem(Transform item)
    {
        collectedItems.Add(item);
        item.SetParent(stackHolder);
        item.localRotation = Quaternion.identity;
        item.localScale = Vector3.one;
        RebuildStackVisuals();
        if (collectSound) audioSource.PlayOneShot(collectSound);
    }

    private void RebuildStackVisuals()
    {
        for (int i = 0; i < collectedItems.Count; i++)
        {
            int reverse = collectedItems.Count - 1 - i;
            collectedItems[i].localPosition = new Vector3(0, reverse * verticalOffset, 0);
        }
    }

    public int GetStackCount() => collectedItems.Count;

    public int GetStackCount(CollectibleKind kind) =>
        collectedItems.Count(t => t && t.GetComponent<CollectibleType>()?.type == kind);

    public bool ContainsCollectibleType(CollectibleKind kind) =>
        collectedItems.Any(t => t && t.GetComponent<CollectibleType>()?.type == kind);

    public void StartUnloadLimited(Transform dropPoint, float offsetY, Func<bool> stillInZone, int limit, Action<Transform> onItemDropped)
    {
        if (collectedItems.Count > 0)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateUnload(dropPoint, offsetY, stillInZone, limit, onItemDropped, null));
        }
    }

    public void StartUnloadOfType(Transform dropPoint, float offsetY, Func<bool> stillInZone, int limit, CollectibleKind kind, Action<Transform> onItemDropped)
    {
        if (ContainsCollectibleType(kind))
        {
            StopAllCoroutines();
            StartCoroutine(AnimateUnload(dropPoint, offsetY, stillInZone, limit, onItemDropped, kind));
        }
    }

    private IEnumerator AnimateUnload(Transform dropPoint, float offsetY, Func<bool> stillInZone, int limit, Action<Transform> onItemDropped, CollectibleKind? filterKind)
    {
        var toDropList = new List<Transform>();
        foreach (var t in collectedItems)
        {
            if (t == null) continue;
            if (filterKind == null || t.GetComponent<CollectibleType>()?.type == filterKind)
            {
                toDropList.Add(t);
                if (toDropList.Count >= limit) break;
            }
        }

        for (int i = 0; i < toDropList.Count; i++)
        {
            if (!stillInZone()) yield break;

            var item = toDropList[i];
            if (item == null) continue;

            item.SetParent(null);
            Vector3 start = item.position;
            Vector3 target = dropPoint.position + Vector3.up * offsetY * i;

            float t = 0f;
            while (t < 1f)
            {
                if (!stillInZone()) yield break;
                item.position = Vector3.Lerp(start, target, t);
                t += Time.deltaTime / 0.3f;
                yield return null;
            }

            item.position = target;
            if (dropSound) audioSource.PlayOneShot(dropSound);
            onItemDropped?.Invoke(item);
            yield return new WaitForSeconds(slowTimeBetweenDrops);
        }

        foreach (var dropped in toDropList)
            collectedItems.Remove(dropped);

        RebuildStackVisuals();
    }
}
