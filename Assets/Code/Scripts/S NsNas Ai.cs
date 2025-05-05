using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class sNassnasAI : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public float detectRange = 8f;
    public float attackRange = 1.8f;

    private NavMeshAgent agent;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
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
            agent.SetDestination(transform.position); // وقف
            animator.SetBool("isWalking", false);
            StartCoroutine(AttackLoop());
        }
        else if (distance <= detectRange)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
        }
        else
        {
            agent.SetDestination(transform.position); // وقف
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
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
