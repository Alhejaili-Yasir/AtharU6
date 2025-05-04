using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public MonoBehaviour movementScript; // اسحب سكربت الحركة هنا

    public float attackDuration = 0.6f; // مدة أنميشن الضرب

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool isInAir = animator.GetBool("air");
            bool isCrouching = animator.GetBool("crouch");

            if (!isInAir && !isCrouching)
            {
                animator.SetBool("attack", true);
                movementScript.enabled = false; // 🔒 وقف الحركة
                StartCoroutine(ResetAttack());
            }
        }
    }

    System.Collections.IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackDuration); // ⏳ نفس مدة الأنميشن
        animator.SetBool("attack", false);
        movementScript.enabled = true; // 🔓 شغل الحركة
    }
}
