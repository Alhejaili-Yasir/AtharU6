using UnityEngine;

public class HomingFireball : MonoBehaviour
{
    public float initialSpeed = 6f;
    public float homingSpeed = 8f;
    public float rotateSpeed = 10f;
    public float delayBeforeHoming = 0.6f;
    public float randomAngleRange = 25f; // الزاوية العشوائية
    public float damage = 10f;
    public GameObject explosionEffect;

    private Rigidbody rb;
    private Transform target;
    private bool isHoming = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 🔀 أضف زاوية انحراف عشوائية حول المحور Y و X
        Quaternion randomRotation = Quaternion.Euler(
            Random.Range(-randomAngleRange, randomAngleRange),
            Random.Range(-randomAngleRange, randomAngleRange),
            0f
        );

        Vector3 launchDir = randomRotation * transform.forward;
        rb.linearVelocity = launchDir * initialSpeed;

        Invoke(nameof(StartHoming), delayBeforeHoming);
    }

    void StartHoming()
    {
        isHoming = true;
    }

    void FixedUpdate()
    {
        if (!isHoming || target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Time.fixedDeltaTime, 0f);

        rb.linearVelocity = newDir * homingSpeed;
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Fireball") || other.isTrigger)
            return;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (other.CompareTag("Player"))
        {
            Debug.Log("🎯 اللاعب أُصيب!");
            // أضف ضرر هنا إذا فيه سكربت صحة
        }

        Destroy(gameObject);
    }
}
