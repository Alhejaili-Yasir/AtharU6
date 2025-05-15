using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName = "Unnamed Item";
    public float pickupRange = 2f;
    public GameObject promptUI; // 👈 UI يظهر فوق العنصر عند الاقتراب
    public AudioClip pickupSound; // 🔊 صوت الالتقاط

    private Transform player;
    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (promptUI != null)
            promptUI.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
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
                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                QuestManager.Instance.AddItem(itemName);

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
