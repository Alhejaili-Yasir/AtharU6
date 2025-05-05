using UnityEngine;

public class HomingFireball : MonoBehaviour
{
    public float initialSpeed = 6f;
    public float homingSpeed = 8f;
    public float rotateSpeed = 10f;
    public float delayBeforeHoming = 0.6f;
    public float randomAngleRange = 25f;
    public float damage = 10f;
    public GameObject explosionEffect;

    private Rigidbody rb;
    private Transform target;
    private Transform reflectTarget;
    private bool isHoming = false;
    private bool isReflected = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 🔍 ابحث عن فانوس بالتاق
        GameObject found = GameObject.FindGameObjectWithTag("Fanos");
        if (found != null)
            reflectTarget = found.transform;

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
        if (other.CompareTag("Reflect") && !isReflected && reflectTarget != null)
        {
            isReflected = true;
            target = reflectTarget;
            Debug.Log("🔥 Reflected! Heading to Fanos.");
            return;
        }

        if (other.CompareTag("Enemy") || other.CompareTag("Fireball") || other.isTrigger)
            return;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (!isReflected && other.CompareTag("Player"))
        {
            Debug.Log("🎯 اللاعب أُصيب!");
        }

        Destroy(gameObject);
    }
}
