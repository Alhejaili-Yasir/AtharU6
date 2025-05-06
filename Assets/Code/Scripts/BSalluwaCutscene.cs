using UnityEngine;
using System.Collections;

public class FollowSaluwaCam : MonoBehaviour
{
    public float activeDuration = 6f;

    void OnEnable()
    {
        StartCoroutine(DeactivateAfterDelay());
    }

    IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(activeDuration);
        gameObject.SetActive(false);
        Debug.Log("📷 الكاميرا تعطلت بعد انتهاء المدة.");
    }
}
