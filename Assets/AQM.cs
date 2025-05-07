using UnityEngine;

public class QuestArrowManager : MonoBehaviour
{
    public ArrowTargetController arrow;

    [Header("NPCs")]
    public Transform npc1; // الأم
    public Transform npc2; // أحمد
    public Transform npc3; // الرجل
    public Transform npc4; // الشيخ

    [Header("Quest Zones")]
    public Transform zone1;
    public Transform zone2;
    public Transform zone3;
    public Transform zone4;

    void Update()
    {
        var qm = QuestManager.Instance;
        Quest current = qm.GetCurrentQuest();
        int index = qm.currentQuestIndex;

        if (current == null) return;

        bool hasItem = qm.GetCollectedAmount(current.requiredItem) >= current.requiredAmount;

        switch (index)
        {
            case 0:
                if (!current.questAccepted)
                    arrow.SetTarget(npc1); // بداية المهمة
                else if (!current.isCompleted && !hasItem)
                    arrow.SetTarget(zone1); // اجمع
                else if (!current.isCompleted && hasItem)
                    arrow.SetTarget(npc1); // رجع سلّم
                else
                    arrow.SetTarget(npc2); // بعد التسليم
                break;

            case 1:
                if (!current.questAccepted)
                    arrow.SetTarget(npc2);
                else if (!current.isCompleted && !hasItem)
                    arrow.SetTarget(zone2);
                else if (!current.isCompleted && hasItem)
                    arrow.SetTarget(npc2);
                else
                    arrow.SetTarget(npc3);
                break;

            case 2:
                if (!current.questAccepted)
                    arrow.SetTarget(npc3);
                else if (!current.isCompleted && !hasItem)
                    arrow.SetTarget(zone3);
                else if (!current.isCompleted && hasItem)
                    arrow.SetTarget(npc3);
                else
                    arrow.SetTarget(npc4);
                break;

            case 3:
                if (!current.questAccepted)
                    arrow.SetTarget(npc4);
                else if (!current.isCompleted && !hasItem)
                    arrow.SetTarget(zone4);
                else if (!current.isCompleted && hasItem)
                    arrow.SetTarget(npc4);
                else
                    arrow.DisableArrow(); // كل المهام خلصت
                break;
        }
    }
}
