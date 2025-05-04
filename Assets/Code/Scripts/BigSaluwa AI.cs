using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BigSaluwaAI : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public float detectRange = 25f;
    public float attackRange = 3f;
    public float patrolRadius = 12f;
    public float walkSpeed = 3.5f;
    public float runSpeed = 6f;

    private NavMeshAgent agent;
    private Vector3 patrolTarget;
    private bool isAttacking = false;
    private float patrolCooldown = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = 12f;
        agent.angularSpeed = 300f;
        PickNewPatrolTarget();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (isAttacking)
        {
            if (distance > attackRange)
            {
                StopCoroutine("AttackLoop");
                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }

            agent.SetDestination(transform.position);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            return;
        }

        if (distance <= attackRange)
        {
            agent.SetDestination(transform.position);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            StartCoroutine(AttackLoop());
        }
        else if (distance <= detectRange)
        {
            agent.speed = runSpeed;
            agent.SetDestination(player.position);

            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            agent.speed = walkSpeed;
            patrolCooldown -= Time.deltaTime;

            if (Vector3.Distance(transform.position, patrolTarget) < 1f || patrolCooldown <= 0f)
            {
                PickNewPatrolTarget();
                patrolCooldown = 3f;
            }

            agent.SetDestination(patrolTarget);

            bool isMoving = agent.velocity.magnitude > 0.1f;

            animator.SetBool("isWalking", isMoving);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
        }
    }

    IEnumerator AttackLoop()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        while (true)
        {
            yield return new WaitForSeconds(1.2f);
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > attackRange) break;
        }

        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    void PickNewPatrolTarget()
    {
        Vector3 randomDir = Random.insideUnitSphere * patrolRadius + transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
