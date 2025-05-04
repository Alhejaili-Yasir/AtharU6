using UnityEngine;

public class BigSaluwaHealth : MonoBehaviour
{
    public int maxHits = 10;
    private int currentHits = 0;
    private bool isDead = false;

    public float destroyDelay = 0.2f;
    public GameObject respawnPrefab;
    public Transform spawnPoint; // 🆕 تحدد المكان يدويًا من Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("player melee"))
        {
            currentHits++;
            Debug.Log("💥 ضربات: " + currentHits);

            if (currentHits >= maxHits)
            {
                isDead = true;
                Debug.Log("💀 السعلوة الكبير مات!");

                if (respawnPrefab != null && spawnPoint != null)
                {
                    Instantiate(respawnPrefab, spawnPoint.position, spawnPoint.rotation);
                }

                Destroy(gameObject, destroyDelay);
            }
        }
    }
}
