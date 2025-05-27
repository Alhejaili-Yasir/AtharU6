using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class FadePanel : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel; // «·»«‰·
    public TextMeshProUGUI text; // «·‰’
    public float fadeDuration = 5f; // «·„œ… ·“Ì«œ… «·‘›«›Ì…
    public float inactiveDelay = 15f; // «·Êﬁ  «·–Ì Ì„— ﬁ»· √‰ Ì’»Õ «·√Ê»Ãﬂ  €Ì— ‰‘ÿ
    private CanvasGroup panelCanvasGroup;
    private CanvasGroup textCanvasGroup;

    void Start()
    {
        // ≈‰‘«¡ CanvasGroup ··»«‰ÌÊ· Ê«· ﬂ”  ≈–« ·„ Ìﬂ‰ „ÊÃÊœ«
        panelCanvasGroup = panel.GetComponent<CanvasGroup>();
        if (panelCanvasGroup == null)
            panelCanvasGroup = panel.AddComponent<CanvasGroup>();

        textCanvasGroup = text.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null)
            textCanvasGroup = text.AddComponent<CanvasGroup>();

        // »œ√ «· ·«‘Ì  œ—ÌÃÌ«
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        // “Ì«œ… «·‘›«›Ì… ··»œ«Ì…
        yield return StartCoroutine(FadeTo(1f, fadeDuration)); // «·‘›«›Ì… „‰ 0 ≈·Ï 1
        yield return new WaitForSeconds(1f); // «·«‰ Ÿ«— ·„œ… 1 À«‰Ì… »Ì‰ «· ·«‘ÌÌ‰

        //  ﬁ·Ì· «·‘›«›Ì… »⁄œ «·«‰ Ÿ«—
        yield return StartCoroutine(FadeTo(0f, fadeDuration)); // «·‘›«›Ì… „‰ 1 ≈·Ï 0

        // «·«‰ Ÿ«— ·„œ… 15 À«‰Ì… ﬁ»· Ã⁄· «·√Ê»Ãﬂ  €Ì— ‰‘ÿ
        yield return new WaitForSeconds(inactiveDelay);

        // Ã⁄· «·√Ê»Ãﬂ  «·–Ì ⁄·ÌÂ «·”ﬂ—»  €Ì— ‰‘ÿ »⁄œ 15 À«‰Ì…
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

        //  √ﬂÌœ «·Ê’Ê· ··Âœ› «·‰Â«∆Ì
        panelCanvasGroup.alpha = targetAlpha;
        textCanvasGroup.alpha = targetAlpha;
    }
}
