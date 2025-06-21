using UnityEngine;
using TMPro;

public class CoinPopup : MonoBehaviour
{
    public TMP_Text popupText;
    public float floatSpeed = 1f;
    public float lifeTime = 1f;

    private float timer = 0f;
    private Vector3 startPos;

    void Awake()
    {
        startPos = transform.localPosition;
        popupText.enabled = false;
    }

    public void Show(string message)
    {
        popupText.text = message;
        popupText.enabled = true;
        timer = lifeTime;
        transform.localPosition = startPos; // Reset position
    }

    void Update()
    {
        if (!popupText.enabled) return;

        timer -= Time.deltaTime;
        transform.localPosition += Vector3.up * floatSpeed * Time.deltaTime;

        if (timer <= 0f)
        {
            popupText.enabled = false;
        }
    }
}