using UnityEngine;

public class ArrowTargetController : MonoBehaviour
{
    public Transform player;                // اللاعب
    public Transform currentTarget;         // الهدف الحالي
    public GameObject arrowObject;          // السهم الذي يشير

    private bool isActive = true;

    void Update()
    {
        if (!isActive || currentTarget == null || player == null) return;

        Vector3 direction = currentTarget.position - player.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
        SetArrowActive(true);
    }

    public void SetArrowActive(bool value)
    {
        isActive = value;
        if (arrowObject != null)
            arrowObject.SetActive(value);
    }
}
