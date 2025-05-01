using UnityEngine;

public class HomingFireball : MonoBehaviour
{
    public float speed = 0.2f;
    public float rotateSpeed = 15f;
    public GameObject explosionEffect;
    public float damage = 10f;

    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        dir.Normalize();

        Vector3 rotateAmount = Vector3.Cross(transform.forward, dir);
        GetComponent<Rigidbody>().angularVelocity = rotateAmount * rotateSpeed;
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Fireball") || other.isTrigger)
            return;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (other.CompareTag("Player"))
        {
            Debug.Log("اللاعب أُصيب!");
            Destroy(gameObject);
        }

        
    }
}
