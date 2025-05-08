using UnityEngine;
using TMPro;

public class QuestUITextUpdater : MonoBehaviour
{
    public TextMeshProUGUI questText;

    void Update()
    {
        if (QuestManager.Instance == null || questText == null)
            return;

        Quest current = QuestManager.Instance.GetCurrentQuest();

        if (current != null && current.questAccepted && !current.isCompleted)
        {
            int collected = QuestManager.Instance.GetCollectedAmount(current.requiredItem);
            questText.text = $"Quest: {current.requiredItem} {collected}/{current.requiredAmount}";
        }
        else
        {
            questText.text = "Follow the arrow";
        }
    }
}
