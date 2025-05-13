using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class FinalCutscene : MonoBehaviour
{
    public GameObject finalScreen;
    public TextMeshProUGUI finalText;
    public AudioSource finalMusic;
    public AudioSource voiceAudioSource;
    public Button mainMenuButton;

    [Header("Cutscene Text and Audio")]
    [TextArea(3, 5)] public List<string> lines = new List<string>();
    public List<AudioClip> lineVoices = new List<AudioClip>();
    public float delayAfterVoice = 1f;
    public float startDelay = 8f; // ⏱️ تأخير البداية (قابل للتعديل من الـInspector)

    [Header("Deactivate on Start")]
    public List<GameObject> objectsToDisable = new List<GameObject>();

    void OnEnable()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        StartCoroutine(ShowEndingSequence());
    }

    IEnumerator ShowEndingSequence()
    {
        finalMusic?.Play();
        finalText.text = "";

        for (int i = 0; i < lines.Count; i++)
        {
            finalText.text = lines[i];

            if (i < lineVoices.Count && lineVoices[i] != null)
            {
                voiceAudioSource.clip = lineVoices[i];
                voiceAudioSource.Play();
                yield return new WaitWhile(() => voiceAudioSource.isPlaying);
            }

            yield return new WaitForSeconds(delayAfterVoice);
        }

        mainMenuButton.gameObject.SetActive(true);
    }
}
