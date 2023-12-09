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
    public float wanderTimer = 5f;
    public float chaseRange = 10f;
    public float attackRange = 3f;
    public int attackPower = 10;
    public Transform player;
    public Vector3 containmentAreaCenter;
    public float containmentRadius = 20f;

    private Vector3 wanderTarget;
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
            yield return new WaitForSeconds(3f);
        }

        isAttacking = false;
    }


    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void SetNewRandomDestination()
    {
        wanderTarget = GetRandomPointInContainmentArea();
        currentState = EnemyState.Wander;
        Invoke("SetNewRandomDestination", wanderTimer);
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
