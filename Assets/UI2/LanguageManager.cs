using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public GameObject[] englishImages; // صور النصوص بالإنجليزية
    public GameObject[] arabicImages;  // صور النصوص بالعربية

    public void SetLanguageEnglish()
    {
        foreach (GameObject go in englishImages)
            go.SetActive(true);

        foreach (GameObject go in arabicImages)
            go.SetActive(false);
    }

    public void SetLanguageArabic()
    {
        foreach (GameObject go in englishImages)
            go.SetActive(false);

        foreach (GameObject go in arabicImages)
            go.SetActive(true);
    }

    // تستخدم إذا كنتِ تختارين اللغة من Dropdown
    public void OnLanguageDropdownChanged(int option)
    {
        if (option == 0)
        {
            SetLanguageArabic();   // الخيار الأول = عربي
        }
        else if (option == 1)
        {
            SetLanguageEnglish(); // الخيار الثاني = إنجليزي
        }
    }
}
