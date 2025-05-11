using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // لو تستخدمين TextMeshPro

public class KeyRebinder : MonoBehaviour
{
    public TMP_Text keyText; // النص المعروض على الزر
    public string actionName; // اسم الإجراء (مثلاً: "Jump", "MoveLeft"...)

    private bool isWaitingForKey = false;

    private void Update()
    {
        if (isWaitingForKey)
        {
            foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc))
                {
                    PlayerPrefs.SetString(actionName, kc.ToString());
                    keyText.text = kc.ToString();
                    isWaitingForKey = false;
                    break;
                }
            }
        }
    }

    public void StartRebind()
    {
        isWaitingForKey = true;
        keyText.text = "...";
    }

    void Start()
    {
        // تحميل المفتاح من PlayerPrefs
        string savedKey = PlayerPrefs.GetString(actionName, "None");
        keyText.text = savedKey;
    }
}