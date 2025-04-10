using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float triggerForce = 0.1f; // Lowered for better reliability
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private GameObject particles;

    private float particleScaleMultiplier = 1f;

    public void SetExplosionForce(float newForce)
    {
        explosionForce = newForce;
        Debug.Log($"[ExplosionScript] Explosion force set to {newForce}");
    }

    public void SetExplosionScale(float chargePercent)
    {
        particleScaleMultiplier = Mathf.Lerp(0.08f, 3f, chargePercent);
        Debug.Log($"[ExplosionScript] Particle scale set to {particleScaleMultiplier} based on charge: {chargePercent}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;
        Debug.Log($"[ExplosionScript] Collision detected with {collision.collider.name}. Impact force: {impactForce}");

        if (impactForce >= triggerForce)
        {
            Debug.Log("[ExplosionScript] Trigger force met, triggering explosion!");

            var surroundingObjects = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var obj in surroundingObjects)
            {
                var rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    Debug.Log($"[ExplosionScript] Applied explosion force to {obj.name}");
                }
            }

            if (particles != null)
            {
                GameObject p = Instantiate(particles, transform.position, Quaternion.identity);
                p.transform.localScale *= particleScaleMultiplier;
                Destroy(p, 3f); // Destroy particle effect after 3 seconds
                Debug.Log("[ExplosionScript] Spawned explosion particles.");
            }

            Debug.Log($"[ExplosionScript] Destroying {gameObject.name}");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[ExplosionScript] Impact force too low to trigger explosion.");
        }
    }
}