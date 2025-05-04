using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public MonoBehaviour movementScript;
    public GameObject activeObject;       // 👈 الأوبجكت اللي تبي يظهر
    public float attackDuration = 0.6f;   // ⏱️ نفس مدة أنميشن الضرب

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool isInAir = animator.GetBool("air");
            bool isCrouching = animator.GetBool("crouch");

            if (!isInAir && !isCrouching)
            {
                animator.SetBool("attack", true);
                movementScript.enabled = false;
                activeObject.SetActive(true);  // ✅ فعل الأوبجكت

                StartCoroutine(ResetAttack());
            }
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackDuration);
        animator.SetBool("attack", false);
        movementScript.enabled = true;
        activeObject.SetActive(false);   // ⛔ رجع الأوبجكت غير مفعل
    }
}
