using UnityEngine;
using System.Collections;

public class CutsceneCameraMover : MonoBehaviour
{
    public Transform[] targets;
    public float moveSpeed = 1.5f;

    public float distanceBehind = 5f;
    public float heightAbove = 3f;
    public float lookDownAngle = 35f;

    private void OnEnable()
    {
        StartCoroutine(MoveThroughPoints());
    }

    IEnumerator MoveThroughPoints()
    {
        foreach (Transform target in targets)
        {
            Vector3 offsetDirection = -target.forward * distanceBehind + Vector3.up * heightAbove;
            Vector3 desiredPosition = target.position + offsetDirection;

            while (Vector3.Distance(transform.position, desiredPosition) > 0.05f)
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * moveSpeed);

                Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                Quaternion tilt = Quaternion.Euler(lookDownAngle, targetRotation.eulerAngles.y, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, tilt, Time.deltaTime * moveSpeed);

                yield return null;
            }

            // تأكد من الوضع النهائي لكل نقطة
            transform.position = desiredPosition;
            Quaternion finalRotation = Quaternion.LookRotation(target.position - transform.position);
            Quaternion finalTilt = Quaternion.Euler(lookDownAngle, finalRotation.eulerAngles.y, 0);
            transform.rotation = finalTilt;

            // ❌ حذفنا: yield return new WaitForSeconds(waitTime);
        }

        Debug.Log("✅ Cutscene camera finished movement.");
    }
}
