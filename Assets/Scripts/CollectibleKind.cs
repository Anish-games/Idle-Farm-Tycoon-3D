using UnityEngine;

public enum CollectibleKind { Old, New }

public class CollectibleType : MonoBehaviour
{
    public CollectibleKind type = CollectibleKind.Old;
    public void SetType(CollectibleKind t) => type = t;
}
