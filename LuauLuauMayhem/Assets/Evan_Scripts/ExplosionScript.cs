    using UnityEngine;

    public class ExplosionScript : MonoBehaviour
    {
        [Header("Explosion Settings")]
        [SerializeField] private float triggerForce = 0.1f;
        [SerializeField] private float explosionRadius = 10f;
        [SerializeField] private float explosionForce = 500f;
        [SerializeField] private GameObject particles;

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip explosionClip;

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
        Debug.Log($"[ExplosionScript] Collision with {collision.collider.name}. Impact force: {impactForce}");

        if (impactForce >= triggerForce)
        {
            Debug.Log("[ExplosionScript] Trigger force met — BOOM!");

            // 🔊 Spawn audio source to survive after this object is destroyed
            if (explosionClip)
            {
                GameObject tempAudio = new GameObject("TempExplosionAudio");
                tempAudio.transform.position = transform.position;

                AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
                tempSource.clip = explosionClip;
                tempSource.spatialBlend = 1f; // 3D sound
                tempSource.minDistance = 1f;
                tempSource.maxDistance = 50f; // tweak as needed
                tempSource.rolloffMode = AudioRolloffMode.Logarithmic; // most realistic
                tempSource.Play();

                Destroy(tempAudio, explosionClip.length); // cleanup
            }


            var surroundingObjects = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (var obj in surroundingObjects)
            {
                var rb = obj.attachedRigidbody;
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                    var enemy = rb.GetComponent<EnemyAiTutorial>();
                    if (enemy != null)
                    {
                        Debug.Log($"[ExplosionScript] Killing enemy: {enemy.name}");
                        enemy.TakeDamage(9999); // Instant kill
                    }
                }
            }

            if (particles != null)
            {
                GameObject p = Instantiate(particles, transform.position, Quaternion.identity);
                p.transform.localScale *= particleScaleMultiplier;
                Destroy(p, 3f);
            }

            Destroy(gameObject); // 💥 Bullet destroyed, sound lives on briefly
        }
        else
        {
            Debug.Log("[ExplosionScript] Impact too soft. No boom.");
        }
    }


}
