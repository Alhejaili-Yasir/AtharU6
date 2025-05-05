using UnityEngine;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject playerCameraObject;
    public Camera cutsceneCamera;
    public ThirdPersonController movementScript;
    public Canvas[] canvasesToToggle;

    [Tooltip("Duration of cutscene in seconds")]
    public float cutsceneDuration = 22.5f;       // ⏱️ مدة الكت سين

    private bool cutscenePlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (cutscenePlayed) return;

        if (other.CompareTag("Player"))
        {
            cutscenePlayed = true;

            Debug.Log("🎬 Cutscene Started");

            if (playerCameraObject != null) playerCameraObject.SetActive(false);
            if (cutsceneCamera != null) cutsceneCamera.gameObject.SetActive(true);
            if (movementScript != null) movementScript.enabled = false;

            foreach (Canvas c in canvasesToToggle)
                if (c != null) c.enabled = false;

            StartCoroutine(EndCutsceneAfterDelay(cutsceneDuration));
        }
    }

    IEnumerator EndCutsceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (cutsceneCamera != null) cutsceneCamera.gameObject.SetActive(false);
        if (playerCameraObject != null) playerCameraObject.SetActive(true);
        if (movementScript != null) movementScript.enabled = true;

        foreach (Canvas c in canvasesToToggle)
            if (c != null) c.enabled = true;

        Debug.Log("✅ Cutscene Ended");
    }
}
