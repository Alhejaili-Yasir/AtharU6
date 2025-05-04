using UnityEngine;
using System.Collections;

public class ZoneMusicTrigger : MonoBehaviour
{
    public AudioSource villageMusic;
    public AudioSource desertMusic;
    public float fadeDuration = 1.5f;

    private Coroutine currentFade;

    void Start()
    {
        villageMusic.volume = 1f;
        desertMusic.volume = 0f;
        villageMusic.Play();
        desertMusic.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentFade != null) StopCoroutine(currentFade);
            currentFade = StartCoroutine(FadeMusic(villageMusic, desertMusic));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentFade != null) StopCoroutine(currentFade);
            currentFade = StartCoroutine(FadeMusic(desertMusic, villageMusic));
        }
    }

    IEnumerator FadeMusic(AudioSource fadeIn, AudioSource fadeOut)
    {
        float timer = 0f;
        float startIn = fadeIn.volume;
        float startOut = fadeOut.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            fadeIn.volume = Mathf.Lerp(startIn, 1f, t);
            fadeOut.volume = Mathf.Lerp(startOut, 0f, t);

            yield return null;
        }

        fadeIn.volume = 1f;
        fadeOut.volume = 0f;
    }
}
