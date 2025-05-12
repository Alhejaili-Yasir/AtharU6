using UnityEngine;

public class ArrowTargetController : MonoBehaviour
{
    public GameObject arrowVisual;
    public float rotationSpeed = 5f;
    public float heightOffset = 2f;

    private Transform currentTarget;
    private GameObject currentIndicator;

    public void SetTarget(Transform target)
    {
        currentTarget = target;
        EnableArrow();

        // إطفاء جميع مؤشرات الـ NPC أولاً
        foreach (NPCIndicator indicator in FindObjectsOfType<NPCIndicator>())
        {
            indicator.Toggle(false);
        }

        // تفعيل مؤشر الـ NPC الحالي فقط
        NPCIndicator ind = target.GetComponent<NPCIndicator>();
        if (ind != null)
        {
            ind.Toggle(true);
            currentIndicator = ind.indicatorObject; // ✅ حفظ المؤشر فقط وليس شخصية كاملة
        }
    }

    public void EnableArrow()
    {
        if (arrowVisual != null)
            arrowVisual.SetActive(true);
    }

    public void DisableArrow()
    {
        if (arrowVisual != null)
            arrowVisual.SetActive(false);
        currentTarget = null;

        if (currentIndicator != null)
            currentIndicator.SetActive(false); // ✅ إيقاف المؤشر فقط
    }

    void Update()
    {
        if (currentTarget == null) return;

        Vector3 targetPos = currentTarget.position;
        targetPos.y = transform.position.y;

        Vector3 direction = targetPos - transform.position;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
        }
    }

    // ✅ الدالة المطلوبة
    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }
}
