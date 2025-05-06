using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName = "Unnamed Item";
    public float pickupRange = 2f;
    public GameObject promptUI; // 👈 UI يظهر فوق العنصر عند الاقتراب

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pickupRange)
        {
            if (promptUI != null)
                promptUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                // ✅ أضف العنصر لنظام المهام المدمج مع الإنفنتوري
                QuestManager.Instance.AddItem(itemName);

                // ✅ إزالة العنصر من العالم
                Destroy(gameObject);
            }
        }
        else
        {
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
