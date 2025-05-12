using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnvironmentChanger : MonoBehaviour
{
    [Header("Skybox & Lighting")]
    public Material newSkybox;
    public Light directionalLight;
    public Color newLightColor = Color.white;
    public float newLightIntensity = 1f;
    public Color newFogColor = Color.gray;

    [Header("Fade UI")]
    public Image fadeImage; // ⬅️ UI Image from Canvas
    public float fadeDuration = 1.5f;

    private Material originalSkybox;
    private Color originalLightColor;
    private float originalLightIntensity;
    private Color originalFogColor;

    private void OnEnable()
    {
        originalSkybox = RenderSettings.skybox;
        originalLightColor = directionalLight.color;
        originalLightIntensity = directionalLight.intensity;
        originalFogColor = RenderSettings.fogColor;

        StartCoroutine(FadeAndChangeEnvironment());
    }

    IEnumerator FadeAndChangeEnvironment()
    {
        // Fade to black
        if (fadeImage != null)
            yield return StartCoroutine(FadeImage(0f, 1f));

        // Change environment
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float lerp = t / fadeDuration;

            if (directionalLight != null)
            {
                directionalLight.color = Color.Lerp(originalLightColor, newLightColor, lerp);
                directionalLight.intensity = Mathf.Lerp(originalLightIntensity, newLightIntensity, lerp);
            }

            RenderSettings.fogColor = Color.Lerp(originalFogColor, newFogColor, lerp);
            yield return null;
        }

        if (newSkybox != null)
            RenderSettings.skybox = newSkybox;

        // Fade back to clear
        if (fadeImage != null)
            yield return StartCoroutine(FadeImage(1f, 0f));
    }

    IEnumerator FadeImage(float from, float to)
    {
        float time = 0f;
        Color c = fadeImage.color;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(from, to, time / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, to);
    }
}
