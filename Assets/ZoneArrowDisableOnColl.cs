using UnityEngine;

public class QuestZoneTrigger : MonoBehaviour
{
    private Collider triggerCollider;
    private bool playerInside = false;
    private ArrowTargetController arrow;

    void Start()
    {
        arrow = FindObjectOfType<ArrowTargetController>();
        triggerCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Quest current = QuestManager.Instance.GetCurrentQuest();
            bool hasAllItems = QuestManager.Instance.GetCollectedAmount(current.requiredItem) >= current.requiredAmount;

            if (!hasAllItems)
            {
                arrow.DisableArrow();
            }
            else
            {
                arrow.EnableArrow();
                Destroy(triggerCollider); // ✅ حذف الكوليدر مباشرة
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && playerInside)
        {
            Quest current = QuestManager.Instance.GetCurrentQuest();
            bool hasAllItems = QuestManager.Instance.GetCollectedAmount(current.requiredItem) >= current.requiredAmount;

            if (hasAllItems)
            {
                arrow.EnableArrow();
                Destroy(triggerCollider); // ✅ حذف الكوليدر إذا ما انحذف
            }
        }
    }
}
