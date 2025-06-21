using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    private ObjectPool pool;
    public void SetPool(ObjectPool p) => pool = p;
    public void ReturnToPool()
    {
        if (pool != null) pool.ReturnToPool(gameObject);
        else Destroy(gameObject);
    }
}
