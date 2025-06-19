using UnityEngine;

public class DropZone : MonoBehaviour
{
    public Transform dropPoint;
    public float dropOffsetY = 0.6f;

    private bool playerInZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered drop zone!");
            playerInZone = true;

            StackCollector collector = other.GetComponentInChildren<StackCollector>();
            if (collector != null)
            {
                collector.StartUnloadIfInZone(dropPoint, dropOffsetY, () => playerInZone);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left drop zone.");
            playerInZone = false;
        }
    }
}
