using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;  // ← إضافة هذا لاستيراد Slider
using System.Collections;

public class NassnasAI : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Transform player;
    public NavMeshAgent agent;

    [Header("Ranges")]
    public float detectRange = 8f;
    public float attackRange = 1.8f;

    [Header("Audio")]
    public AudioSource walkAudio;
    public AudioSource attackAudio;

    [Header("UI")]
    public Slider healthSlider; // اسحب السلايدر من الـInspector

    private bool isAttacking = false;
    private bool isWalkingSoundPlaying = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // تشغيل/إيقاف صوت المشي حسب الحركة
        if (agent.velocity.magnitude > 0.1f)
        {
            if (!isWalkingSoundPlaying && walkAudio != null)
            {
                walkAudio.Play();
                isWalkingSoundPlaying = true;
            }
        }
        else
        {
            if (isWalkingSoundPlaying && walkAudio != null)
            {
                walkAudio.Stop();
                isWalkingSoundPlaying = false;
            }
        }

        // إذا في هجمة جارية
        if (isAttacking)
        {
            if (distance > attackRange)
            {
                StopCoroutine("AttackLoop");
                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }
            return;
        }

        // بدء هجمة
        if (distance <= attackRange)
        {
            agent.SetDestination(transform.position); // وقف
            animator.SetBool("isWalking", false);
            StartCoroutine(AttackLoop());
        }
        // مطاردة اللاعب
        else if (distance <= detectRange)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
        }
        // الوقوف إذا بعيد جداً
        else
        {
            agent.SetDestination(transform.position);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
        }
    }

    IEnumerator AttackLoop()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        if (attackAudio != null)
            attackAudio.Play();

        while (true)
        {
            // انتظار دورة الهجوم
            yield return new WaitForSeconds(1f);

            // نقص 3 من السلايدر في كل ضربة
            if (healthSlider != null)
                healthSlider.value = Mathf.Max(healthSlider.minValue, healthSlider.value - 3f);

            // تحقق من المسافة لاستمرار الهجوم
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > attackRange)
                break;
        }

        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
