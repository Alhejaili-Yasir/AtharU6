using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SaluwaAI : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public float detectRange = 10f;
    public float attackRange = 2f;
    public float walkRadius = 5f;

    private NavMeshAgent agent;
    private Vector3 walkTarget;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PickNewWalkTarget();
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
            return;
        }

        if (distance <= attackRange)
        {
            agent.SetDestination(transform.position);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            StartCoroutine(AttackLoop());
        }
        else if (distance <= detectRange)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
        else
        {
            if (Vector3.Distance(transform.position, walkTarget) < 1f)
            {
                PickNewWalkTarget();
            }
            agent.SetDestination(walkTarget);
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
    }

    IEnumerator AttackLoop()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        while (true)
        {
            yield return new WaitForSeconds(1f);
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > attackRange) break;
        }

        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    void PickNewWalkTarget()
    {
        Vector3 randomDir = Random.insideUnitSphere * walkRadius;
        randomDir += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, walkRadius, NavMesh.AllAreas))
        {
            walkTarget = hit.position;
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
    }
}
