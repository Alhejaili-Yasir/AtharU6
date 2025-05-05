using UnityEngine;

public class FanousHealth : MonoBehaviour
{
    public int hitsToDestroy = 5;
    public GameObject abuFanous;        // الوحش الرئيسي
    public GameObject itemToSpawn;      // الأوبجكت اللي يرسبن بعد موته
    public Transform spawnPoint;        // المكان اللي يرسبن فيه الأوبجكت

    private int hitCount = 0;
    private bool isDestroyed = false;

    void OnTriggerEnter(Collider other)
    {
        if (isDestroyed) return;

        if (other.CompareTag("Fireball"))
        {
            hitCount++;
            Debug.Log($"💥 Fanous hit! ({hitCount}/{hitsToDestroy})");

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
            Destroy(abuFanous); // أو حط أنميشن موته بداله
        }

        if (itemToSpawn != null && spawnPoint != null)
        {
            Instantiate(itemToSpawn, spawnPoint.position, spawnPoint.rotation);
        }

        Destroy(gameObject); // دمر الفانوس نفسه
    }
}
