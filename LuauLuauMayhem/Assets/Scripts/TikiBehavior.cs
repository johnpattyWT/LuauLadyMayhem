using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour
{
    public Transform player;

    public LayerMask whatIsPlayer;

    public float health;

    // Movement
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    // Patroling
    public Vector3 patrolPoint;
    private bool patrolPointSet;
    public float patrolRange = 20f;
    public float patrolHeightRange = 5f;

    // Attacking
    public float minAttackDelay = 2f;
    public float maxAttackDelay = 6f;
    private bool alreadyAttacked;
    public GameObject projectile;
    public Transform projectileSpawnPoint;

    // States
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    // Orbiting
    [Header("Orbit Settings")]
    public float orbitDistance = 5f;
    public float orbitSpeed = 50f;
    private float orbitAngle;

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
        if (!patrolPointSet)
            SearchPatrolPoint();

        if (patrolPointSet)
        {
            MoveTowards(patrolPoint);

            if (Vector3.Distance(transform.position, patrolPoint) < 1f)
                patrolPointSet = false;
        }

        if (audioSource && patrolClip && !audioSource.isPlaying)
            audioSource.PlayOneShot(patrolClip);
    }

    private void SearchPatrolPoint()
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomY = Random.Range(-patrolHeightRange, patrolHeightRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        patrolPoint = transform.position + new Vector3(randomX, randomY, randomZ);
        patrolPointSet = true;
    }

    private void ChasePlayer()
    {
        MoveTowards(player.position);

        if (audioSource && chaseClip && !audioSource.isPlaying)
            audioSource.PlayOneShot(chaseClip);
    }

    private void AttackPlayer()
    {
        OrbitAroundPlayer();
        LookAtPlayer();

        if (!alreadyAttacked)
        {
            Transform spawn = projectileSpawnPoint != null ? projectileSpawnPoint : transform;
            GameObject proj = Instantiate(projectile, spawn.position, Quaternion.identity);
            Rigidbody rb = proj.GetComponent<Rigidbody>();

            Vector3 direction = (player.position - spawn.position).normalized;
            rb.AddForce(direction * 32f, ForceMode.Impulse);

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

    private void OrbitAroundPlayer()
    {
        if (player == null) return;

        orbitAngle += orbitSpeed * Time.deltaTime;
        float radians = orbitAngle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians)) * orbitDistance;
        Vector3 targetPosition = player.position + offset;

        // Smoothly follow player's height (hovering effect)
        targetPosition.y = Mathf.Lerp(transform.position.y, player.position.y, 0.05f);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sightRange);
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