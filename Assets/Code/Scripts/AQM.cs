using UnityEngine;

public class QuestArrowManager : MonoBehaviour
{
    public ArrowTargetController arrow;

    [Header("NPCs")]
    public Transform npc1;
    public Transform npc2;
    public Transform npc3;
    public Transform npc4;

    [Header("Quest Zones")]
    public Transform zone1;
    public Transform zone2;
    public Transform zone3;
    public Transform zone4;
    public Transform shopZone;

    [Header("Reflect Shield Tracking")]
    public GameObject reflectShieldObject; // 👈 اسحب هنا الأوبجكت اللي يشتغل لما تشتري Reflect Shield

    void Update()
    {
        var qm = QuestManager.Instance;
        Quest current = qm.GetCurrentQuest();
        int index = qm.currentQuestIndex;

        if (current == null) return;

        bool hasItem = qm.GetCollectedAmount(current.requiredItem) >= current.requiredAmount;
        bool hasShield = reflectShieldObject != null && reflectShieldObject.activeInHierarchy;

        switch (index)
        {
            case 0:
                if (!current.questAccepted)
                    arrow.SetTarget(npc1);
                else if (!current.isCompleted && !hasItem)
                    arrow.SetTarget(zone1);
                else if (!current.isCompleted && hasItem)
                    arrow.SetTarget(npc1);
                else
                    arrow.SetTarget(npc2);
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
                {
                    arrow.SetTarget(npc4);
                }
                else if (!current.isCompleted && !hasItem)
                {
                    if (!hasShield)
                        arrow.SetTarget(shopZone); // اشتري Reflect Shield
                    else
                        arrow.SetTarget(zone4); // عندك الدرع
                }
                else if (!current.isCompleted && hasItem)
                {
                    arrow.SetTarget(npc4);
                }
                else
                {
                    arrow.DisableArrow();
                }
                break;
        }
    }
}
