using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float minAttackDelay = 2f;
    public float maxAttackDelay = 6f;
    bool alreadyAttacked;
    public GameObject projectile;
    public Transform projectileSpawnPoint;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Audio
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip patrolClip;
    public AudioClip chaseClip;
    public AudioClip attackClip;
    public AudioClip hurtClip;
    public AudioClip deathClip;

    private void Awake()
    {
        player = GameObject.Find("TempPlayer").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInAttackRange && playerInSightRange)
            AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        if (audioSource && patrolClip && !audioSource.isPlaying)
            audioSource.PlayOneShot(patrolClip);
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        if (audioSource && chaseClip && !audioSource.isPlaying)
            audioSource.PlayOneShot(chaseClip);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Transform spawn = projectileSpawnPoint != null ? projectileSpawnPoint : transform;
            GameObject proj = Instantiate(projectile, spawn.position, Quaternion.identity);
            Rigidbody rb = proj.GetComponent<Rigidbody>();

            Vector3 baseDirection = transform.forward;
            Vector3 toPlayer = (player.position - spawn.position).normalized;
            Vector3 finalDirection = (baseDirection * 0.7f + toPlayer * 0.3f).normalized;

            rb.AddForce(finalDirection * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 2f, ForceMode.Impulse);

            Bullet homing = proj.GetComponent<Bullet>();
            if (homing != null)
            {
                homing.target = player;
            }

            alreadyAttacked = true;
            float randomAttackDelay = Random.Range(minAttackDelay, maxAttackDelay);
            Invoke(nameof(ResetAttack), randomAttackDelay);

            if (audioSource && attackClip)
                audioSource.PlayOneShot(attackClip);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (audioSource && hurtClip)
            audioSource.PlayOneShot(hurtClip);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
