using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName = "عنصر بدون اسم";
    public float pickupRange = 2f;

    private Transform player;
    public GameObject promptUI; // 👈 النص العلوي (يتم ربطه من Inspector)

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // نخفي النص في البداية
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
                InventorySystem.Instance.AddItem(itemName);
                Destroy(gameObject); // نخفي العنصر
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
