using UnityEngine;

public class LanternWeakPoint : MonoBehaviour
{
    public AbuFanousAI abuFanousAI;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Debug.Log("الفانوس ضُرب!");
            abuFanousAI.DestroyLantern();
        }
    }
}
