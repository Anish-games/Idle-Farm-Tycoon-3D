using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCollectibleSpawner : MonoBehaviour
{
    public ObjectPool oldCollectiblePool;
    public int numberToMaintain = 10;
    public Transform pointA, pointB, pointC, pointD;
    public float checkInterval = 2f; // how often to check and respawn

    private List<GameObject> activeCollectibles = new();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            CleanUpInactive();

            int toSpawn = numberToMaintain - activeCollectibles.Count;
            if (toSpawn > 0)
            {
                for (int i = 0; i < toSpawn; i++)
                {
                    SpawnOne();
                }
            }
        }
    }

    void SpawnOne()
    {
        Vector3 pos = GetRandomPosition();
        GameObject obj = oldCollectiblePool.GetFromPool(pos, Quaternion.identity);

        var collectible = obj.GetComponent<CollectibleType>();
        if (collectible != null)
            collectible.SetType(CollectibleKind.Old);

        activeCollectibles.Add(obj);
    }

    void CleanUpInactive()
    {
        activeCollectibles.RemoveAll(obj => obj == null || !obj.activeInHierarchy);
    }

    Vector3 GetRandomPosition()
    {
        float minX = Mathf.Min(pointA.position.x, pointB.position.x, pointC.position.x, pointD.position.x);
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x, pointC.position.x, pointD.position.x);
        float minZ = Mathf.Min(pointA.position.z, pointB.position.z, pointC.position.z, pointD.position.z);
        float maxZ = Mathf.Max(pointA.position.z, pointB.position.z, pointC.position.z, pointD.position.z);
        float y = pointA.position.y;
        return new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
    }
}
