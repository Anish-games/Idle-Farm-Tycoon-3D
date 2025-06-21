using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProcessingFactory : MonoBehaviour
{
    [Header("Pools")]
    public ObjectPool oldCollectiblePool;
    public ObjectPool newCollectiblePool;

    [Header("Settings")]
    public float processingTime = 10f;
    public Transform spawnPoint;
    public float verticalOffset = 2f;

    [Header("UI")]
    public TextMeshProUGUI countdownText;

    private bool isProcessing;

    public bool CanProcess() => !isProcessing;

    public void StartProcessing(List<Transform> inputCollectibles, System.Action onComplete)
    {
        if (isProcessing || inputCollectibles.Count < 2) return;
        StartCoroutine(Process(inputCollectibles, onComplete));
    }

    private IEnumerator Process(List<Transform> inputCollectibles, System.Action onComplete)
    {
        isProcessing = true;

        // 1) Countdown
        float t = processingTime;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            if (countdownText) countdownText.text = $"Processing: {Mathf.CeilToInt(t)}s";
            yield return null;
        }
        if (countdownText) countdownText.text = "";

        // 2) Return old items to pool
        foreach (var tr in inputCollectibles)
            if (tr != null)
                tr.GetComponent<PoolableObject>()?.ReturnToPool();

        // 3) Spawn new items (1 per 2 old)
        int toSpawn = inputCollectibles.Count / 2;
        for (int i = 0; i < toSpawn; i++)
        {
            Vector3 pos = spawnPoint.position + Vector3.up * verticalOffset * i;
            var obj = newCollectiblePool.GetFromPool(pos, spawnPoint.rotation);
            obj.GetComponent<CollectibleType>()?.SetType(CollectibleKind.New);
        }

        isProcessing = false;
        onComplete?.Invoke();
    }
}
