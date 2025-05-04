using UnityEngine;
using UnityEngine.AI;

public class RandomVillageWalker : MonoBehaviour
{
    public float minWaitTime = 3f;
    public float maxWaitTime = 6f;
    public float minWalkRadius = 10f;  // ⬅️ مسافات أكبر
    public float maxWalkRadius = 25f;  // ⬅️ مسافات أبعد
    public float walkSpeed = 4f;

    private NavMeshAgent agent;
    private Animator animator;
    private float waitTimer = 0f;
    private float currentWaitTime = 0f;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = walkSpeed;
        agent.angularSpeed = 500f;
        agent.acceleration = 20f;
        agent.stoppingDistance = 0.1f;

        PickNewDestination();
    }

    void Update()
    {
        agent.speed = walkSpeed;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                animator.SetBool("isWalking", false);
                isWaiting = true;
                currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
                waitTimer = 0f;
            }

            waitTimer += Time.deltaTime;
            if (waitTimer >= currentWaitTime)
            {
                PickNewDestination();
                isWaiting = false;
            }
        }
        else
        {
            animator.SetBool("isWalking", true);
            isWaiting = false;
        }
    }

    void PickNewDestination()
    {
        float radius = Random.Range(minWalkRadius, maxWalkRadius);
        Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
