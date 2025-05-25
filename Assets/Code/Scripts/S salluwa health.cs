using UnityEngine;
using UnityEngine.UI;

public class SaluwaHealth : MonoBehaviour
{
    public int maxHits = 3;
    private int currentHits = 0;
    private bool isDead = false;

    public float destroyDelay = 0.2f;
    public GameObject respawnPrefab;
    public Transform spawnPoint;

    public Slider healthSlider;
    public Transform player;
    public float showRange = 15f;

    public ParticleSystem bloodEffect;     // تأثير الدم
    public Transform bloodSpawnPoint;      // نقطة ظهور الدم

    private void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHits;
            healthSlider.value = maxHits;
            healthSlider.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (healthSlider != null && player != null && !isDead)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            healthSlider.gameObject.SetActive(distance <= showRange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("player melee"))
        {
            currentHits++;
            Debug.Log("ضربة #" + currentHits);

            if (healthSlider != null)
            {
                healthSlider.value = maxHits - currentHits;
            }

            if (bloodEffect != null && bloodSpawnPoint != null)
            {
                bloodEffect.transform.position = bloodSpawnPoint.position;
                bloodEffect.transform.rotation = bloodSpawnPoint.rotation;
                bloodEffect.Play();
            }

            if (currentHits >= maxHits)
            {
                isDead = true;
                Debug.Log("💀 السعلوة الصغيرة ماتت");

                if (respawnPrefab != null && spawnPoint != null)
                {
                    Instantiate(respawnPrefab, spawnPoint.position, spawnPoint.rotation);
                }

                Destroy(gameObject, destroyDelay);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, showRange);
    }
}
