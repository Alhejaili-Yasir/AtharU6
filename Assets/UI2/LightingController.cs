using UnityEngine;
using UnityEngine.UI;

public class LightingController : MonoBehaviour
{
    public Light sceneLight; // ربط مكون الإضاءة من الـInspector
    public Slider lightSlider; // ربط سلايدر الإضاءة من الـInspector

    void Start()
    {
        // تأكد من أن السلايدر يضبط قيمة الإضاءة بشكل صحيح عند بدء اللعبة
        if (lightSlider != null && sceneLight != null)
        {
            // ضبط الحد الأقصى والأدنى للإضاءة بناءً على السلايدر
            lightSlider.minValue = 0f; // أقل قيمة للإضاءة (0 = ظلام تام)
            lightSlider.maxValue = 2f; // أعلى قيمة للإضاءة (2 = أقصى إضاءة)

            // تحديث الإضاءة بناءً على قيمة السلايدر
            lightSlider.value = sceneLight.intensity; // تعيين قيمة السلايدر بناءً على شدة الإضاءة الحالية
            lightSlider.onValueChanged.AddListener(OnLightSliderChanged); // إضافة مستمع لتغيير القيمة
        }
    }

    // هذه الدالة سيتم استدعاؤها عندما يتم تغيير قيمة السلايدر
    public void OnLightSliderChanged(float value)
    {
        // تحديث شدة الإضاءة بناءً على القيمة التي تم اختيارها في السلايدر
        if (sceneLight != null)
        {
            sceneLight.intensity = value;
        }
    }
}
