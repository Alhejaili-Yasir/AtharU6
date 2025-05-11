using UnityEngine;
using UnityEngine.UI;

public class GraphicsManager : MonoBehaviour
{
    // هذه المتغيرات ستمثل الـDropdowns أو الـSliders في الـUI
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle shadowToggle;
    public Toggle lightingToggle;

    void Start()
    {
        // قم بملء الخيارات في الـDropdown والـToggles عند بداية اللعبة
        InitializeSettings();
    }

    // قم بتهيئة الإعدادات عند بدء اللعبة
    private void InitializeSettings()
    {
        // إعداد الـResolution
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(GetAvailableResolutions());

        // إعداد الـQuality
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(GetAvailableQualities());

        // إعداد الظلال والإضاءة بناءً على الخيارات الحالية
        shadowToggle.isOn = QualitySettings.shadowCascades > 0;
        lightingToggle.isOn = RenderSettings.ambientLight != Color.black;
    }

    // الحصول على قائمة الدقات المتاحة
    private System.Collections.Generic.List<string> GetAvailableResolutions()
    {
        System.Collections.Generic.List<string> resolutions = new System.Collections.Generic.List<string>();

        foreach (Resolution res in Screen.resolutions)
        {
            resolutions.Add(res.width + "x" + res.height);
        }

        return resolutions;
    }

    // الحصول على قائمة الجودة المتاحة
    private System.Collections.Generic.List<string> GetAvailableQualities()
    {
        System.Collections.Generic.List<string> qualities = new System.Collections.Generic.List<string>();

        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            qualities.Add(QualitySettings.names[i]);
        }

        return qualities;
    }

    // تغيير دقة الشاشة
    public void ChangeResolution(int index)
    {
        Resolution resolution = Screen.resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // تغيير جودة الرسوميات
    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    // تفعيل/إلغاء الظلال
    public void ToggleShadows(bool isOn)
    {
        QualitySettings.shadowCascades = isOn ? 2 : 0; // تغيير الظلال بين On/Off
    }

    // تفعيل/إلغاء الإضاءة
    public void ToggleLighting(bool isOn)
    {
        RenderSettings.ambientLight = isOn ? Color.white : Color.black; // تغيير الإضاءة بين On/Off
    }
}
