using UnityEngine;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    public int questIndex;
    public GameObject questPanel;
    public TextMeshProUGUI questText;
    public TextMeshProUGUI floatingText;

    [TextArea]
    public string idleText = "Hello there!";
    public string talkHint = "Click E to talk";

    private bool playerInRange = false;
    private bool questGiven = false;
    private bool hasInteracted = false;

    void Start()
    {
        if (floatingText != null)
            floatingText.text = idleText;
    }

    void Update()
    {
        if (playerInRange && floatingText != null && !hasInteracted)
            floatingText.text = talkHint;

        if (!playerInRange && floatingText != null)
            floatingText.text = idleText;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            hasInteracted = true;

            Quest currentQuest = QuestManager.Instance.GetCurrentQuest();

            if (questIndex < QuestManager.Instance.currentQuestIndex)
            {
                questPanel.SetActive(true);
                questText.text = "✅ You already completed this quest.";
                return;
            }

            if (questIndex > QuestManager.Instance.currentQuestIndex)
            {
                questPanel.SetActive(true);

                if (questIndex - 1 >= 0 && questIndex - 1 < QuestManager.Instance.quests.Count)
                {
                    questText.text = $"❌ Finish the previous quest: {QuestManager.Instance.quests[questIndex - 1].questName}";
                }
                else
                {
                    questText.text = "❌ This quest is not available yet.";
                }

                return;
            }

            if (!questGiven)
            {
                questPanel.SetActive(true);
                questText.text = $"📜 Quest: Collect {currentQuest.requiredAmount} of {currentQuest.requiredItem}.";
                questGiven = true;
                return;
            }

            if (!currentQuest.isCompleted)
            {
                int collected = QuestManager.Instance.GetCollectedAmount(currentQuest.requiredItem);
                int remaining = currentQuest.requiredAmount - collected;

                questPanel.SetActive(true);

                if (remaining > 0)
                {
                    questText.text = $"📦 You still need {remaining} more {currentQuest.requiredItem}.";
                }
                else
                {
                    currentQuest.isCompleted = true;
                    QuestManager.Instance.RemoveItem(currentQuest.requiredItem, currentQuest.requiredAmount);
                    QuestManager.Instance.AddMoney(currentQuest.rewardMoney);
                    QuestManager.Instance.MoveToNextQuest();

                    questText.text = $"✅ Quest completed! You earned {currentQuest.rewardMoney} coins.";
                }
            }
            else
            {
                questPanel.SetActive(true);
                questText.text = "✅ You already completed this quest.";
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
            hasInteracted = false; // ✅ نرجعه false عند دخول الرينج من جديد
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            questPanel.SetActive(false);
            hasInteracted = false;
        }
    }
}
