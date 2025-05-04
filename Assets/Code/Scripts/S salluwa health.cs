using UnityEngine;

public class SaluwaHealth : MonoBehaviour
{
    public int maxHits = 3;
    private int currentHits = 0;
    private bool isDead = false;

    public float destroyDelay = 0.2f;
    public GameObject respawnPrefab;
    public Transform spawnPoint; // 👈 اختر نقطة الرسبنة

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("player melee"))
        {
            currentHits++;
            Debug.Log("ضربة #" + currentHits);

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
}
