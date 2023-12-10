using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Wander,
        Chase,
        Attack,
        Flee
        // Add more states as needed
    }

    public EnemyState currentState;
    public float health = 100f;
    public float speed = 5f;
    public float chaseRange = 10f;
    public float attackRange = 3f;
    public int attackPower = 10;
    public Transform player;
    public Vector3 containmentAreaCenter;
    public float containmentRadius = 20f;

    private Vector3 wanderTarget;
    private float wanderTimer;
    private bool isAttacking = false;

    void Start()
    {
        currentState = EnemyState.Wander;
        Invoke("SetNewRandomDestination", wanderTimer);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Wander:
                MoveTowards(wanderTarget);
                CheckChaseConditions();
                break;
            case EnemyState.Chase:
                MoveTowards(player.position);
                CheckChaseConditions();
                break;
            case EnemyState.Attack:
                PerformAttack();
                break;
            case EnemyState.Flee:
                // Implement flee logic
                break;
        }
    }

    void CheckChaseConditions()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
        {
            if (currentState != EnemyState.Chase)
            {
                currentState = EnemyState.Chase;
                Debug.Log("Chase State");
            }
        }
        else if (distanceToPlayer > chaseRange)
        {
            if (currentState != EnemyState.Wander)
            {
                currentState = EnemyState.Wander;
                Debug.Log("Wander State");
                SetNewRandomDestination();
            }
        }
        else if (distanceToPlayer <= attackRange)
        {
            if (currentState != EnemyState.Attack)
            {
                currentState = EnemyState.Attack;
                Debug.Log("Attack State");
            }
        }
    }

    void PerformAttack()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackPlayer(attackPower));
        }
    }

    IEnumerator AttackPlayer(int damage)
    {
        isAttacking = true;

        while (currentState == EnemyState.Attack && player != null)
        {
            player.GetComponent<PlayerFishController>().TakeDamage(damage);
            yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
        }

        isAttacking = false;
    }


    void MoveTowards(Vector3 target)
    {
        // Calculate direction to the target.
        Vector3 direction = (target - transform.position).normalized;

        // Check if the next movement will keep the fish within the containment area.
        Vector3 nextPosition = transform.position + direction * speed * Time.deltaTime;
        Vector3 directionToCenter = containmentAreaCenter - nextPosition;

        if (directionToCenter.magnitude > containmentRadius)
        {
            // If the fish is going out of bounds, steer it back towards the containment center.
            Vector3 newDirection = (containmentAreaCenter - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            // If the fish is within bounds, continue towards the target.
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void SetNewRandomDestination()
    {
        wanderTarget = GetRandomPointInContainmentArea();
        currentState = EnemyState.Wander;
        Invoke("SetNewRandomDestination", Random.Range(3.0f, 5.0f));
    }

    Vector3 GetRandomPointInContainmentArea()
    {
        Vector3 randomDirection = Random.insideUnitSphere * containmentRadius;
        randomDirection += containmentAreaCenter;

        randomDirection.x = Mathf.Clamp(randomDirection.x, containmentAreaCenter.x - containmentRadius, containmentAreaCenter.x + containmentRadius);
        randomDirection.z = Mathf.Clamp(randomDirection.z, containmentAreaCenter.z - containmentRadius, containmentAreaCenter.z + containmentRadius);

        return randomDirection;
    }
}
