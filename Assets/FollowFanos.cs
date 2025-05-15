using UnityEngine;

public class FlyingLanternPath : MonoBehaviour
{
    public Transform player;
    public Transform[] pathPoints;
    public float detectionRadius = 5f;
    public float moveSpeed = 2f;
    public float rotationSmooth = 3f;

    private int currentIndex = 0;
    private bool isMoving = false;

    void Update()
    {
        if (player == null || pathPoints.Length == 0) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ✅ يتحرك للنقطة التالية فقط إذا اللاعب دخل المدى ولم نصل للنهاية
        if (distanceToPlayer <= detectionRadius && !isMoving && currentIndex < pathPoints.Length - 1)
        {
            currentIndex++;
            isMoving = true;
        }

        // ✅ حركة نحو النقطة الحالية
        Vector3 targetPos = pathPoints[currentIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // ✅ إيقاف الحركة إذا وصل للنقطة
        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            isMoving = false;

            // ✅ إذا وصلنا للنقطة الأخيرة، دمر الفانوس
            if (currentIndex == pathPoints.Length - 1)
            {
                Destroy(gameObject);
            }
        }

        // ✅ دوران ناعم نحو النقطة التالية (إن وجدت)
        if (currentIndex < pathPoints.Length - 1)
        {
            Vector3 lookDir = pathPoints[currentIndex + 1].position - transform.position;
            lookDir.y = 0;
            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSmooth);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

        if (pathPoints != null && pathPoints.Length > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < pathPoints.Length - 1; i++)
            {
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
            }
        }
    }
}
