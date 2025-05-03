using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    void Update()
    {
        if (PlayerStats.Instance != null && moneyText != null)
        {
            moneyText.text = "Money: " + PlayerStats.Instance.money;
        }
    }
}
