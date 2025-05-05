using UnityEngine;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    public int questIndex;
    public GameObject questPanel;
    public TextMeshProUGUI questText;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Quest currentQuest = QuestManager.Instance.GetCurrentQuest();

            if (QuestManager.Instance.currentQuestIndex != questIndex)
            {
                questPanel.SetActive(true);

                if (questIndex - 1 >= 0 && questIndex - 1 < QuestManager.Instance.quests.Count)
                {
                    questText.text = $"Finish the previous quest: {QuestManager.Instance.quests[questIndex - 1].questName}";
                }
                else
                {
                    questText.text = "This quest is not available yet.";
                }

                return;
            }

            if (!currentQuest.isCompleted)
            {
                questPanel.SetActive(true);
                questText.text = $"Collect {currentQuest.requiredAmount} of {currentQuest.requiredItem}.";
            }
            else
            {
                questText.text = $"Well done! You earned {currentQuest.rewardMoney} money.";
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
            playerInRange = true;
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
