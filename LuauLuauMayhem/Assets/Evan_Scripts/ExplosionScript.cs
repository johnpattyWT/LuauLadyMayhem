using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] private float triggerForce = 0.5f;
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private GameObject particles;

    private float particleScaleMultiplier = 1f;

    public void SetExplosionForce(float newForce)
    {
        explosionForce = newForce;
    }

    public void SetExplosionScale(float chargePercent)
    {
        particleScaleMultiplier = Mathf.Lerp(1f, 3f, chargePercent); // Scale from 1x to 3x
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= triggerForce)
        {
            var surroundingObjects = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (var obj in surroundingObjects)
            {
                var rb = obj.GetComponent<Rigidbody>();
                if (rb == null) continue;

                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            if (particles != null)
            {
                GameObject p = Instantiate(particles, transform.position, Quaternion.identity);
                p.transform.localScale *= particleScaleMultiplier;
            }

            Destroy(gameObject);
        }
    }
}