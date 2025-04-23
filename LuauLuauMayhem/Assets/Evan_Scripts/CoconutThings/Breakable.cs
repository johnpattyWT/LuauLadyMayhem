using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Break Settings")]
    [SerializeField] private GameObject _replacement;  // Fragment prefab
    [SerializeField] private float _breakForce = 2f;
    [SerializeField] private float _collisionMultiplier = 100f;
    [SerializeField] private LayerMask breakFromLayers;  // Can be used to specify the layers that can break this object
    [SerializeField] private bool _broken;

    // Force break manually triggered by explosion, passing explosion force
    public void ForceBreak(float explosionForce)
    {
        if (_broken) return;
        _broken = true;

        // Instantiate fragments
        var replacement = Instantiate(_replacement, transform.position, transform.rotation);

        // Apply explosion force to the fragments based on the explosion impact
        var rbs = replacement.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            // Apply dynamic explosion force based on actual explosion force
            rb.AddExplosionForce(explosionForce, transform.position, 2f);
        }

        // Destroy the fragments after 10 seconds
        Destroy(replacement, 10f);
        Destroy(gameObject);  // Destroy the original object
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_broken) return;

        // Only break if the impact is from an object with the right layer and force is sufficient
        if ((breakFromLayers.value & (1 << collision.gameObject.layer)) == 0) return;

        if (collision.relativeVelocity.magnitude >= _breakForce)
        {
            ForceBreak(collision.relativeVelocity.magnitude);  // Pass actual collision force to ForceBreak
        }
    }
}