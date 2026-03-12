using UnityEngine;

public class ProjectileSystem : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public GameObject effectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);
        
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out ActorController actor)) actor.TakeDamage(50);
            if (hit.TryGetComponent(out Rigidbody rb)) rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
        Destroy(gameObject);
    }
}