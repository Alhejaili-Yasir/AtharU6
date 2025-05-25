using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SliderWatcher : MonoBehaviour
{
    [Header("References")]
    public Slider slider;           // ���� ��� ��� Slider �� ��� Inspector
    public GameObject gameOverUI;   // ���� ��� ��� UI ���� ��� ����

    [Header("Settings")]
    public float displayDuration = 2f; // ��� ���� ��� UI ��� ����� ������

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
