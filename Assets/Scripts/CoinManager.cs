using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("Coin UI")]
    public TextMeshProUGUI totalCoinText;
    public CoinPopup coinPopup;

    [Header("Settings")]
    public int coinsPerCollectible = 10;

    private int totalCoins = 0;

    void Awake()
    {
        Instance = this;
    }

    public void CollectCoin(Vector3 worldPosition)
    {
        totalCoins += coinsPerCollectible;
        UpdateCoinUI();

        if (coinPopup != null)
            coinPopup.Show("+" + coinsPerCollectible);

        
    }

    public bool HasEnoughCoins(int cost)
    {
        return totalCoins >= cost;
    }

    public bool SpendCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            totalCoinText.text = totalCoins.ToString();
            return true;
        }

        return false;
    }


    private void UpdateCoinUI()
    {
        if (totalCoinText != null)
            totalCoinText.text = totalCoins.ToString();
    }
}