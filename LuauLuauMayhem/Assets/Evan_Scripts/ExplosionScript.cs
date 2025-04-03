using System.IO;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] private float triggerForce = 0.5f;
    [SerializeField] private float explosionRadius = 10;
    [SerializeField] private float explosionForce = 500;
    //[SerializeField] private GameObject particles (For Rob)

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.relativeVelocity.magnitude >= triggerForce)
        {
            var surroundingObjects = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (var obj in surroundingObjects)
            {
                var rb = obj.GetComponent<Rigidbody>();
                if (rb == null) continue;

                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            Destroy(gameObject);
        }
    }
}
