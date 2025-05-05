using UnityEngine;
using System.Collections;

public class FollowSaluwaCam : MonoBehaviour
{
    public Transform target;           // السعلوة الكبيرة
    public Vector3 offset = new Vector3(0, 8, -10); // المسافة والزاوية
    public float followDuration = 5f;
    public float transitionTime = 2f;

    private bool isFollowing = false;
    private float timer = 0f;
    private Vector3 startPos;
    private Quaternion startRot;

    void OnEnable()
    {
        if (target == null) return;

        startPos = transform.position;
        startRot = transform.rotation;
        timer = 0f;
        StartCoroutine(TransitionToTarget());
    }

    IEnumerator TransitionToTarget()
    {
        Vector3 targetPos = target.position + target.TransformDirection(offset);
        Quaternion targetRot = Quaternion.LookRotation(target.position - targetPos);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / transitionTime;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        isFollowing = true;
        StartCoroutine(FollowTarget());
    }

    IEnumerator FollowTarget()
    {
        float followTime = 0f;

        while (followTime < followDuration)
        {
            if (target == null) yield break;

            Vector3 desiredPos = target.position + target.TransformDirection(offset);
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * 3f);

            Quaternion desiredRot = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * 3f);

            followTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("📷 كاميرا السعلوة الكبيرة انتهت.");
        isFollowing = false;

        // هنا تقدر تطفي الكاميرا أو ترجّع الكاميرا الأصلية
        // gameObject.SetActive(false);
    }
}
