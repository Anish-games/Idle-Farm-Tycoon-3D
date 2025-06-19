using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingFactory : MonoBehaviour
{
    [Header("Processing Settings")]
    public float processingTime = 10f;
    public int maxProcessedCount = 2; // Editable in Inspector
    public GameObject collectiblePrefab;
    public Transform spawnPoint;
    public float verticalOffset = 2f; // For stacking new collectibles

    private int currentProcessedCount = 0;
    private bool isProcessing = false;

    public bool CanProcess()
    {
        return currentProcessedCount < maxProcessedCount;
    }

    public void StartProcessing(List<Transform> inputCollectibles, System.Action onProcessingComplete)
    {
        if (isProcessing || !CanProcess()) return;

        StartCoroutine(Process(inputCollectibles, onProcessingComplete));
    }

    private IEnumerator Process(List<Transform> inputCollectibles, System.Action onProcessingComplete)
    {
        isProcessing = true;

        Debug.Log("Processing started...");

        yield return new WaitForSeconds(processingTime);

        // Destroy input collectibles
        foreach (Transform item in inputCollectibles)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        // Spawn new collectible
        if (collectiblePrefab && spawnPoint && CanProcess())
        {
            // Count how many new collectibles already exist under spawnPoint
            int currentStackHeight = 0;
            foreach (Transform child in spawnPoint)
            {
                var type = child.GetComponent<CollectibleType>();
                if (type != null && type.type == CollectibleKind.New)
                {
                    currentStackHeight++;
                }
            }

            Vector3 stackPosition = spawnPoint.position + new Vector3(0, currentStackHeight * verticalOffset, 0);

            GameObject newCollectible = Instantiate(collectiblePrefab, stackPosition, spawnPoint.rotation);
            Debug.Log("New collectible spawned at: " + stackPosition);

            // Make sure it is marked as "New"
            CollectibleType newType = newCollectible.GetComponent<CollectibleType>();
            if (newType != null)
            {
                newType.type = CollectibleKind.New;
            }

            currentProcessedCount++;
        }

        isProcessing = false;
        onProcessingComplete?.Invoke();
    }
}
