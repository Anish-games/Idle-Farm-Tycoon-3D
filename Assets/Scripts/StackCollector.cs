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
        item.SetParent(stackHolder);

        // Use inspector-set verticalOffset instead of object's bounds
        item.localPosition = new Vector3(0, stackCount * verticalOffset, 0);
        item.localRotation = Quaternion.identity;
        item.localScale = Vector3.one;

        collectedItems.Add(item);
        stackCount++;

        if (collectSound && audioSource)
            audioSource.PlayOneShot(collectSound);
    }

    // Call this from DropZone and pass a function that checks if player is still in the zone
    public void StartUnloadIfInZone(Transform dropPoint, float offsetY, Func<bool> isPlayerStillInZone)
    {
        if (collectedItems.Count > 0)
        {
            StopAllCoroutines(); // Stop any previous unloads
            StartCoroutine(AnimateUnloadWhileInZone(dropPoint, offsetY, isPlayerStillInZone));
        }
    }

    private IEnumerator AnimateUnloadWhileInZone(Transform dropPoint, float offsetY, Func<bool> isPlayerStillInZone)
    {
        for (int i = 0; i < collectedItems.Count; i++)
        {
            if (!isPlayerStillInZone()) yield break;

            Transform item = collectedItems[i];
            item.SetParent(null);

            Vector3 targetPos = dropPoint.position + new Vector3(0, i * offsetY, 0);
            float duration = 0.3f;
            float elapsed = 0f;
            Vector3 startPos = item.position;

            while (elapsed < duration)
            {
                if (!isPlayerStillInZone()) yield break;

                item.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            item.position = targetPos;

            if (dropSound && audioSource)
                audioSource.PlayOneShot(dropSound);

            yield return new WaitForSeconds(SlowTheTimeBy);
        }

        collectedItems.Clear();
        stackCount = 0;
    }
}
