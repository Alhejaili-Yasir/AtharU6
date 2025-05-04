using UnityEngine;
using UnityEngine.AI;

public class RandomWalker : MonoBehaviour
{
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;
    public float minWalkRadius = 3f;
    public float maxWalkRadius = 8f;
    public float walkSpeed = 3.5f;

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
        // دايم نحدث السرعة من Inspector
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
