using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class FadePanel : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel; // ������
    public TextMeshProUGUI text; // ����
    public float fadeDuration = 5f; // ����� ������ ��������
    public float inactiveDelay = 15f; // ����� ���� ��� ��� �� ���� �������� ��� ���
    private CanvasGroup panelCanvasGroup;
    private CanvasGroup textCanvasGroup;

    void Start()
    {
        // ����� CanvasGroup �������� ������� ��� �� ��� �������
        panelCanvasGroup = panel.GetComponent<CanvasGroup>();
        if (panelCanvasGroup == null)
            panelCanvasGroup = panel.AddComponent<CanvasGroup>();

        textCanvasGroup = text.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null)
            textCanvasGroup = text.AddComponent<CanvasGroup>();

        // ��� ������� ��������
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        // ����� �������� �������
        yield return StartCoroutine(FadeTo(1f, fadeDuration)); // �������� �� 0 ��� 1
        yield return new WaitForSeconds(1f); // �������� ���� 1 ����� ��� ���������

        // ����� �������� ��� ��������
        yield return StartCoroutine(FadeTo(0f, fadeDuration)); // �������� �� 1 ��� 0

        // �������� ���� 15 ����� ��� ��� �������� ��� ���
        yield return new WaitForSeconds(inactiveDelay);

        // ��� �������� ���� ���� ������� ��� ��� ��� 15 �����
        gameObject.SetActive(false);
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = panelCanvasGroup.alpha;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float elapsed = (Time.time - startTime) / duration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed);

            panelCanvasGroup.alpha = alpha;
            textCanvasGroup.alpha = alpha;

            yield return null;
        }

        // ����� ������ ����� �������
        panelCanvasGroup.alpha = targetAlpha;
        textCanvasGroup.alpha = targetAlpha;
    }
}
