using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class BigSaluwaAI : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Transform player;
    public NavMeshAgent agent;

    [Header("Ranges")]
    public float detectRange = 10f;
    public float attackRange = 2f;
    public float walkRadius = 6f;

    [Header("Roam Limit")]
    public Transform roamCenter;
    public float roamLimit = 12f;

    [Header("Audio")]
    public AudioSource detectAudio;
    public AudioSource attackAudio;

    [Header("UI")]
    public Slider healthSlider; // اسحب السلايدر المناسب من الـInspector

    private bool isPlayingDetectAudio = false;
    private bool isPlayingAttackAudio = false;
    private bool isAttacking = false;
    private Vector3 walkTarget;
    private Coroutine roamRoutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        roamRoutine = StartCoroutine(RoamLoop());
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // كشف وصوت
        if (distance <= detectRange && !isPlayingDetectAudio)
        {
            detectAudio.Play();
            isPlayingDetectAudio = true;
        }
        else if (distance > detectRange && isPlayingDetectAudio)
        {
            detectAudio.Stop();
            isPlayingDetectAudio = false;
        }

        // إذا في هجمة جارية
        if (isAttacking)
        {
            if (distance > attackRange)
            {
                StopCoroutine("AttackLoop");
                StopAttackAudio();
                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }
            return;
        }

        // بدء هجمة
        if (distance <= attackRange)
        {
            StopRoaming();
            agent.SetDestination(transform.position);
            animator.SetBool("isAttacking", true);
            StartCoroutine(AttackLoop());
        }
        // مطاردة اللاعب
        else if (distance <= detectRange)
        {
            StopRoaming();
            agent.SetDestination(player.position);
            SetAnim(false, true, false);
        }
        // تجوال
        else
        {
            if (roamRoutine == null)
                roamRoutine = StartCoroutine(RoamLoop());
        }
    }

    IEnumerator RoamLoop()
    {
        while (true)
        {
            if (isAttacking) yield break;

            PickNewWalkTarget();
            agent.SetDestination(walkTarget);

            float walkTime = Random.Range(5f, 8f);
            float walkTimer = 0f;

            while (walkTimer < walkTime)
            {
                if (isAttacking || PlayerInRange()) yield break;

                SetAnim(true, false, false);
                walkTimer += Time.deltaTime;
                yield return null;
            }

            agent.SetDestination(transform.position);
            SetAnim(false, false, false);

            yield return new WaitForSeconds(Random.Range(1.5f, 2f));
        }
    }

    IEnumerator AttackLoop()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        PlayAttackAudio();

        while (true)
        {
            // انتظر دورة الهجوم
            yield return new WaitForSeconds(1f);

            // نقص 5 من السلايدر في كل ضربة
            if (healthSlider != null)
                healthSlider.value = Mathf.Max(healthSlider.minValue, healthSlider.value - 5f);

            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > attackRange)
                break;
        }

        StopAttackAudio();
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    void StopRoaming()
    {
        if (roamRoutine != null)
        {
            StopCoroutine(roamRoutine);
            roamRoutine = null;
        }
    }

    bool PlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= detectRange;
    }

    void PickNewWalkTarget()
    {
        Vector3 center = roamCenter != null ? roamCenter.position : transform.position;
        float minDistance = walkRadius * 0.7f;
        int attempts = 0;

        while (attempts < 10)
        {
            Vector3 randomDir = Random.insideUnitSphere * walkRadius;
            randomDir.y = 0;
            Vector3 candidate = center + randomDir;

            if (Vector3.Distance(center, candidate) <= roamLimit &&
                Vector3.Distance(transform.position, candidate) >= minDistance)
            {
                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 1.5f, NavMesh.AllAreas))
                {
                    walkTarget = hit.position;
                    return;
                }
            }

            attempts++;
        }

        walkTarget = transform.position;
    }

    void SetAnim(bool walk, bool run, bool attack)
    {
        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", walk && isMoving);
        animator.SetBool("isRunning", run && isMoving);
        animator.SetBool("isAttacking", attack);
    }

    void PlayAttackAudio()
    {
        if (!isPlayingAttackAudio && attackAudio != null)
        {
            attackAudio.Play();
            isPlayingAttackAudio = true;
        }
    }

    void StopAttackAudio()
    {
        if (isPlayingAttackAudio && attackAudio != null)
        {
            attackAudio.Stop();
            isPlayingAttackAudio = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, walkRadius);
        if (roamCenter != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(roamCenter.position, roamLimit);
        }
    }
}
