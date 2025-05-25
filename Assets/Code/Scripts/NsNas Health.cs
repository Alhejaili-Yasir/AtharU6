using UnityEngine;
using UnityEngine.UI;

public class NesnasHealth : MonoBehaviour
{
    public int maxHits = 5;
    private int currentHits = 0;
    private bool isDead = false;

    public float destroyDelay = 0.2f;
    public GameObject respawnPrefab;
    public Transform spawnPoint;

    public Slider healthSlider;          // 🆕 شريط الصحة
    public Transform player;             // 🆕 اللاعب
    public float showRange = 15f;        // 🆕 مدى ظهور الشريط
    public ParticleSystem hitEffect;     // 🆕 تأثير الضربة
    public Transform effectSpawnPoint;   // 🆕 نقطة تأثير الضربة
    public AudioSource hitSound;         // 🆕 صوت الضربة
    private bool hasPlayedHitSound = false;

    private void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHits;
            healthSlider.value = maxHits;
            healthSlider.gameObject.SetActive(false); // يبدأ مخفي
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

            if (hitEffect != null && effectSpawnPoint != null)
            {
                hitEffect.transform.position = effectSpawnPoint.position;
                hitEffect.transform.rotation = effectSpawnPoint.rotation;
                hitEffect.Play();
            }

            if (!hasPlayedHitSound && hitSound != null)
            {
                hitSound.Play();
                hasPlayedHitSound = true;
            }

            if (currentHits >= maxHits)
            {
                isDead = true;
                Debug.Log("💀 النسناس مات");

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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, showRange);
    }
}
