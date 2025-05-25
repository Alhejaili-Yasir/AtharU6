using UnityEngine;
using UnityEngine.UI;

public class FanousHealth : MonoBehaviour
{
    public int hitsToDestroy = 5;
    public GameObject abuFanous;
    public GameObject itemToSpawn;
    public Transform spawnPoint;

    public Slider healthSlider;
    public Transform player;
    public float showRange = 15f;

    public ParticleSystem hitEffect;
    public Transform effectSpawnPoint;

    public AudioSource hitSound; // 🔊 صوت الضربة
    private bool hasPlayedHitSound = false;

    private int hitCount = 0;
    private bool isDestroyed = false;

    private void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = hitsToDestroy;
            healthSlider.value = hitsToDestroy;
            healthSlider.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (healthSlider != null && player != null && !isDestroyed)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            healthSlider.gameObject.SetActive(distance <= showRange);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDestroyed) return;

        if (other.CompareTag("Fireball"))
        {
            hitCount++;
            Debug.Log($"💥 Fanous hit! ({hitCount}/{hitsToDestroy})");

            if (healthSlider != null)
            {
                healthSlider.value = hitsToDestroy - hitCount;
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

            if (hitCount >= hitsToDestroy)
            {
                DestroyFanous();
            }
        }
    }

    void DestroyFanous()
    {
        isDestroyed = true;

        if (abuFanous != null)
        {
            Destroy(abuFanous);
        }

        if (itemToSpawn != null && spawnPoint != null)
        {
            Instantiate(itemToSpawn, spawnPoint.position, spawnPoint.rotation);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, showRange);
    }
}
