using UnityEngine;

public class AbuFanousAI : MonoBehaviour
{
    public Transform lantern;
    public GameObject fireballPrefab;
    public Transform fireSpawnPoint;
    public float attackCooldown = 3f;
    public float detectionRange = 15f; // 🔵 النطاق اللي يشتغل فيه أبو فانوس

    private float cooldownTimer;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        cooldownTimer = attackCooldown;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            // ✅ لف باتجاه اللاعب (Y فقط)
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0f; // نخلي الدوران فقط حول Y
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // ✅ إطلاق الكرات
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                ShootFireball();
                cooldownTimer = attackCooldown;
            }
        }
    }

    void ShootFireball()
    {
        if (fireballPrefab != null && fireSpawnPoint != null)
        {
            Instantiate(fireballPrefab, fireSpawnPoint.position, fireSpawnPoint.rotation);
        }
    }

    public void DestroyLantern()
    {
        Debug.Log("تم تدمير فانوس أبو فانوس!");
        Destroy(lantern.gameObject);
        Destroy(gameObject);
    }

    // 🔍 رؤية النطاق داخل Unity Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
