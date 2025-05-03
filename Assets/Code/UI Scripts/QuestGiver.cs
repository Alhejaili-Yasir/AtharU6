using UnityEngine;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    public GameObject questPanel;
    public TextMeshProUGUI questText;
    public string requiredItem = "Meat";
    public int requiredAmount =5;
    public int rewardMoney = 100;

    private bool playerInRange = false;
    private bool questGiven = false;
    private bool questCompleted = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!questGiven)
            {
                questPanel.SetActive(true);
                questText.text = $"Collect {requiredAmount} of {requiredItem}.";
                questGiven = true;
            }
            else if (questGiven && !questCompleted)
            {
                int current = InventorySystem.Instance.GetItemCount(requiredItem);

                if (current >= requiredAmount)
                {
                    InventorySystem.Instance.RemoveItem(requiredItem); 
                    for (int i = 1; i < requiredAmount; i++)
                        InventorySystem.Instance.RemoveItem(requiredItem);

                    PlayerStats.Instance.AddGold(rewardMoney);

                    questText.text = $"Well done! You earned {rewardMoney} Money.";
                    questCompleted = true;
                }
                else
                {
                    int remaining = requiredAmount - current;
                    questText.text = $"You still need {remaining} of {requiredItem}.";
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            questPanel.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            questPanel.SetActive(false);
        }
    }
}
