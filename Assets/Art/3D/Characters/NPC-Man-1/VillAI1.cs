using UnityEngine;
using UnityEngine.AI;

public class RandomVillageWalker : MonoBehaviour
{
    public float minWaitTime = 3f;
    public float maxWaitTime = 6f;
    public float minWalkRadius = 10f;
    public float maxWalkRadius = 25f;
    public float walkSpeed = 4f;

    [Header("Walking Area Limit")]
    public Transform walkCenter;
    public float walkLimit = 30f;

    private NavMeshAgent agent;
    private Animator animator;
    private float waitTimer = 0f;
    private float currentWaitTime = 0f;
    private bool isWaiting = false;

    private float stuckTimer = 0f;
    private Vector3 lastPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = walkSpeed;
        agent.angularSpeed = 500f;
        agent.acceleration = 20f;
        agent.stoppingDistance = 0.1f;
        agent.updateRotation = false; // نوقف الدوران التلقائي

        PickNewDestination();
        lastPosition = transform.position;
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

            // دوران يدوي أثناء المشي فقط
            if (agent.hasPath)
            {
                Vector3 direction = agent.desiredVelocity.normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }
            }

            // كشف إذا الشخصية علقت
            if (Vector3.Distance(transform.position, lastPosition) < 0.05f)
            {
                stuckTimer += Time.deltaTime;
                if (stuckTimer > 2f)
                {
                    PickNewDestination();
                    stuckTimer = 0f;
                }
            }
            else
            {
                stuckTimer = 0f;
            }

            lastPosition = transform.position;
        }
    }

    void PickNewDestination()
    {
        for (int i = 0; i < 10; i++)
        {
            float radius = Random.Range(minWalkRadius, maxWalkRadius);
            Vector3 randomDirection = Random.insideUnitSphere * radius + walkCenter.position;
            randomDirection.y = transform.position.y;

            if (Vector3.Distance(randomDirection, walkCenter.position) <= walkLimit)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (walkCenter != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(walkCenter.position, walkLimit);

            Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
            Gizmos.DrawWireSphere(walkCenter.position, minWalkRadius);
            Gizmos.DrawWireSphere(walkCenter.position, maxWalkRadius);
        }
    }
}
