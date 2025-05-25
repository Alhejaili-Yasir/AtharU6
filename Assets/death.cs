using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SliderWatcher : MonoBehaviour
{
    [Header("References")]
    public Slider slider;           // ÇÓÍÈ åäÇ ÇáÜ Slider ãä ÇáÜ Inspector
    public GameObject gameOverUI;   // ÇÓÍÈ åäÇ ÇáÜ UI Çááí ÊÈí íÙåÑ

    [Header("Settings")]
    public float displayDuration = 2f; // ãÏÉ ÙåæÑ ÇáÜ UI ŞÈá ÅÚÇÏÉ ÇáãÔåÏ

    private bool triggered = false;

    void Start()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (!triggered && slider != null && slider.value <= slider.minValue)
        {
            triggered = true;
            if (gameOverUI != null)
                gameOverUI.SetActive(true);
            StartCoroutine(ShowUIAndRestart());
        }
    }

    private IEnumerator ShowUIAndRestart()
    {
        yield return new WaitForSeconds(displayDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
