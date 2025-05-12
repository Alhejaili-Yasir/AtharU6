using UnityEngine;
using TMPro;

public class QuestUITextUpdater : MonoBehaviour
{
    public TextMeshProUGUI questText;
    public ArrowTargetController arrow; // ⬅️ اربطه في Inspector

    public Transform zone3_5; // ⬅️ اربطه في Inspector

    void Update()
    {
        if (QuestManager.Instance == null || questText == null)
            return;

        Quest current = QuestManager.Instance.GetCurrentQuest();

        // ✅ إذا خلصت كل المهام
        if (current == null)
        {
            questText.text = " Congratulations!";
            return;
        }

        // ✅ إذا المهمة الحالية لم تكتمل
        if (current.questAccepted && !current.isCompleted)
        {
            int collected = QuestManager.Instance.GetCollectedAmount(current.requiredItem);
            questText.text = $"Quest: {current.requiredItem} {collected}/{current.requiredAmount}";

            // ✅ إذا السهم يأشر على منطقة 3.5، أطلب منه شراء الدرع
            if (arrow != null && arrow.GetCurrentTarget() == zone3_5)
            {
                questText.text = "Buy the Reflect Shield!!";
            }

            return;
        }

        // ✅ الحالة الافتراضية
        questText.text = "Follow the arrow";
    }
}
