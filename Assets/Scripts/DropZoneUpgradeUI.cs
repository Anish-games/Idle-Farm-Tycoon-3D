using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropZoneUpgradeUI : MonoBehaviour
{
    public DropZone targetDropZone;
    public Button upgradeButton;
    public TMP_Text capacityText;
    public TMP_Text upgradeCostText;

    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradeDropZone);
        UpdateUI();
    }

    public void ShowUI(DropZone zone)
    {
        targetDropZone = zone;
        gameObject.SetActive(true);
        UpdateUI();
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        if (targetDropZone == null) return;

        int currentCapacity = targetDropZone.capacity;
        int upgradeCost = currentCapacity * 10;

        capacityText.text = $"Current Capacity: {currentCapacity}";
        upgradeCostText.text = $"Upgrade Cost: {upgradeCost} Coins";

        upgradeButton.interactable = CoinManager.Instance.HasEnoughCoins(upgradeCost);
    }

    private void UpgradeDropZone()
    {
        int cost = targetDropZone.capacity * 10;
        if (CoinManager.Instance.SpendCoins(cost))
        {
            targetDropZone.capacity *= 2;
            UpdateUI();
        }
    }
}