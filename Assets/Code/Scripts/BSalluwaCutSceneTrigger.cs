using UnityEngine;

public class BigSaluwaCameraTrigger : MonoBehaviour
{
    public GameObject followCamera;         // الكاميرا اللي تتابع السعلوة الكبيرة
    public GameObject playerCameraObject;   // الكاميرا الأساسية
    public MonoBehaviour playerController;  // سكربت حركة اللاعب (يتم تعطيله مؤقتًا)

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (playerCameraObject != null) playerCameraObject.SetActive(false);
            if (playerController != null) playerController.enabled = false;

            if (followCamera != null)
            {
                followCamera.SetActive(true);
                StartCoroutine(ReturnToPlayerCamera());
            }
        }
    }

    System.Collections.IEnumerator ReturnToPlayerCamera()
    {
        yield return new WaitForSeconds(7f); // 2s انتقال + 5s تتبع

        if (followCamera != null) followCamera.SetActive(false);
        if (playerCameraObject != null) playerCameraObject.SetActive(true);
        if (playerController != null) playerController.enabled = true;

        Debug.Log("✅ رجع للكاميرا الأساسية.");
    }
}
