using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panelToDestroy; // «·»«‰· «·–Ì ”Ì „  œ„Ì—Â
    public GameObject panelToActivate; // «·»«‰· «·–Ì ”Ì „  ›⁄Ì·Â
    public float destroyDelay = 2f; // «·Êﬁ  «·–Ì ”Ì „ »⁄œÂ  œ„Ì— «·»«‰·

    [Header("Keyboard Inputs")]
    public List<KeyCode> activationKeys; // ﬁ«∆„… «·√“—«— «· Ì ”Ì „ «· Õﬁﬁ „‰Â«

    private bool isPanelDestroyed = false; // ·  »⁄ ≈–«  „  œ„Ì— «·»«‰· √„ ·«

    void Start()
    {
        // «· √ﬂœ „‰ √‰ «·»«‰· «·À«‰Ì „›⁄¯· »‘ﬂ·  ·ﬁ«∆Ì
        if (panelToActivate != null)
        {
            panelToActivate.SetActive(false); // ≈Œ›«¡ «·»«‰· «·–Ì ”Ì „  ›⁄Ì·Â
        }
    }

    void Update()
    {
        // «· Õﬁﬁ ≈–«  „ «·÷€ÿ ⁄·Ï √Ì „‰ «·√“—«— «·„Õœœ… ›Ì «·‹ Inspector
        foreach (var key in activationKeys)
        {
            if (Input.GetKeyDown(key) && !isPanelDestroyed) // ≈–«  „ «·÷€ÿ ⁄·Ï “— Ê·„ Ì „  œ„Ì— «·»«‰· »⁄œ
            {
                StartCoroutine(DestroyPanelAfterDelay());
                break; // ·« ‰Õ «Ã ·· Õﬁﬁ „‰ «·√“—«— «·√Œ—Ï »⁄œ  ›⁄Ì· «·‘—ÿ
            }
        }
    }

    // Coroutine · œ„Ì— «·‹ Panel »⁄œ «· √ŒÌ— «·„Õœœ
    IEnumerator DestroyPanelAfterDelay()
    {
        // «·«‰ Ÿ«— ·⁄œœ «·ÀÊ«‰Ì «·„Õœœ
        yield return new WaitForSeconds(destroyDelay);

        //  œ„Ì— «·‹ Panel
        if (panelToDestroy != null)
        {
            panelToDestroy.SetActive(false);
            isPanelDestroyed = true; //  „  œ„Ì— «·»«‰·
        }

        //  ›⁄Ì· «·»«‰· «·À«‰Ì
        if (panelToActivate != null)
        {
            panelToActivate.SetActive(true);
        }
    }
}
