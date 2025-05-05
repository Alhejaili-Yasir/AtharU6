using UnityEngine;
using System.Collections;

public class AbuFanousCutsceneTrigger : MonoBehaviour
{
    public GameObject objectToDisable;     // أي أوبجكت تبي تطفيه
    public GameObject cutsceneCamera;      // الكاميرا الخاصة بالكت سين
    public GameObject playerCamera;        // الكاميرا الأساسية
    public MonoBehaviour playerController; // سكربت حركة اللاعب
    public Canvas[] canvasesToDisable;     // تقدر تحط أكثر من كانفاس

    public float cutsceneDuration = 8f;    // تحددها من Inspector

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (objectToDisable != null) objectToDisable.SetActive(false);
        if (cutsceneCamera != null) cutsceneCamera.SetActive(true);
        if (playerCamera != null) playerCamera.SetActive(false);
        if (playerController != null) playerController.enabled = false;

        foreach (Canvas canvas in canvasesToDisable)
            if (canvas != null) canvas.enabled = false;

        StartCoroutine(EndCutsceneAfterDelay());
    }

    IEnumerator EndCutsceneAfterDelay()
    {
        yield return new WaitForSeconds(cutsceneDuration);

        if (cutsceneCamera != null) cutsceneCamera.SetActive(false);
        if (playerCamera != null) playerCamera.SetActive(true);
        if (playerController != null) playerController.enabled = true;

        foreach (Canvas canvas in canvasesToDisable)
            if (canvas != null) canvas.enabled = true;

        Debug.Log("✅ كت سين أبو فانوس انتهى.");
    }
}
