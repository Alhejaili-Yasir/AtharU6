using UnityEngine;
using System.Collections;

public class PlayerDualAttack : MonoBehaviour
{
    public Animator animator;
    public MonoBehaviour movementScript;

    [Header("Q Attack Settings")]
    public GameObject qActiveObject;
    public float qAttackDuration = 0.6f;

    [Header("R Attack Settings")]
    public GameObject rActiveObject;
    public float rAttackDuration = 0.6f;

    void Update()
    {
        bool isInAir = animator.GetBool("air");
        bool isCrouching = animator.GetBool("crouch");

        if (!isInAir && !isCrouching)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(PerformAttack(qActiveObject, qAttackDuration));
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(PerformAttack(rActiveObject, rAttackDuration));
            }
        }
    }

    IEnumerator PerformAttack(GameObject obj, float duration)
    {
        animator.SetBool("attack", true);
        movementScript.enabled = false;
        if (obj != null) obj.SetActive(true);

        yield return new WaitForSeconds(duration);

        animator.SetBool("attack", false);
        movementScript.enabled = true;
        if (obj != null) obj.SetActive(false);
    }
}
