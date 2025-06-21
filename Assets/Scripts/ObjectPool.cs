using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int initialSize = 10;

    private Queue<GameObject> pool = new();

    void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            var obj = Instantiate(prefab);
            obj.SetActive(false);
            obj.GetComponent<PoolableObject>()?.SetPool(this);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetFromPool(Vector3 pos, Quaternion rot)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab);
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        obj.GetComponent<PoolableObject>()?.SetPool(this);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
