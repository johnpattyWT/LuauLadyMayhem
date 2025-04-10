using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    [Tooltip("How quickly the projectile adjusts its course. Lower values mean a gentler homing effect.")]
    public float homingStrength = 0.5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            // Determine the desired direction toward the target.
            Vector3 desiredDirection = (target.position - transform.position).normalized;
            // Get the current velocity's direction.
            Vector3 currentDirection = rb.linearVelocity.normalized;
            // Interpolate between the current direction and the desired direction.
            Vector3 newDirection = Vector3.Lerp(currentDirection, desiredDirection, homingStrength * Time.fixedDeltaTime).normalized;
            rb.linearVelocity = newDirection * rb.linearVelocity.magnitude;
            // Rotate the projectile to face its movement direction.
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}