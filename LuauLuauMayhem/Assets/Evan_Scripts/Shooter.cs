using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 20f;
    public float fireCooldown = 0.25f;
    public Camera playerCamera;

    private float lastFireTime;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= lastFireTime + fireCooldown)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || shootPoint == null || playerCamera == null)
            return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f)); // center of screen
        Vector3 shootDirection = ray.direction;

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(shootDirection));
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = shootDirection * projectileSpeed;
        }

        lastFireTime = Time.time;
    }
}