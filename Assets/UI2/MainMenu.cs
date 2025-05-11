using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject legendsPanel;

    [Header("Audio Settings")]
    public Slider musicVolumeSlider;
    public Slider characterVoiceSlider;
    public Slider sfxVolumeSlider;

    [Header("Language Settings")]
    public Dropdown languageDropdown;

    [Header("Graphics Settings")]
    public Dropdown resolutionDropdown;
    public Toggle lightingToggle;
    public Toggle shadowsToggle;
    public Toggle nightEffectsToggle;

    void Start()
    {
        // إعداد القيم الافتراضية
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        characterVoiceSlider.onValueChanged.AddListener(OnCharacterVoiceChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

        lightingToggle.onValueChanged.AddListener(OnGraphicsSettingsChanged);
        shadowsToggle.onValueChanged.AddListener(OnGraphicsSettingsChanged);
        nightEffectsToggle.onValueChanged.AddListener(OnGraphicsSettingsChanged);
    }

    // يبدأ اللعبة
    public void StartGame()
    {
        SceneManager.LoadScene("START_GAME");
    }

    // يغلق اللعبة
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // يظهر إعدادات اللعبة
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // يغلق إعدادات اللعبة
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // يظهر بانل Legends
    public void OpenLegends()
    {
        legendsPanel.SetActive(true);
    }

    // يغلق بانل Legends
    public void CloseLegends()
    {
        legendsPanel.SetActive(false);
    }

    // تغيير مستوى صوت الموسيقى
    public void OnMusicVolumeChanged(float value)
    {
        Debug.Log("Music Volume: " + value);
        // AudioManager.Instance.SetMusicVolume(value); // مثال
    }

    // تغيير صوت الشخصيات
    public void OnCharacterVoiceChanged(float value)
    {
        Debug.Log("Character Voice Volume: " + value);
        // AudioManager.Instance.SetVoiceVolume(value);
    }

    // تغيير صوت المؤثرات
    public void OnSFXVolumeChanged(float value)
    {
        Debug.Log("SFX Volume: " + value);
        // AudioManager.Instance.SetSFXVolume(value);
    }

    // تغيير اللغة
    public void OnLanguageChanged(int index)
    {
        string selectedLanguage = languageDropdown.options[index].text;
        Debug.Log("Selected Language: " + selectedLanguage);
        // LocalizationManager.Instance.SetLanguage(selectedLanguage);
    }

    // تغيير إعدادات الرسوميات
    public void OnGraphicsSettingsChanged(bool _)
    {
        Debug.Log("Graphics Settings Changed");
        Debug.Log("Lighting: " + lightingToggle.isOn);
        Debug.Log("Shadows: " + shadowsToggle.isOn);
        Debug.Log("Night Effects: " + nightEffectsToggle.isOn);
        // GraphicsManager.Instance.ApplySettings(...);
    }
}


//   يغلق اللعبة بتطبيق
/* public void QuitGame()
 {
     Debug.Log("Quit Pressed!");
     Application.Quit();
 }*/

