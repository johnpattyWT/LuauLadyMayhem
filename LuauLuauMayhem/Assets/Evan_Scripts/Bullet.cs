using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 10f; // <-- Changed from 5 to 10
    private Vector3 direction;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            direction = transform.forward;
        }

        Destroy(gameObject, lifetime); // Auto-destroy after 10 seconds
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Optional: Add damage here
            Debug.Log("[Bullet] Hit the player!");
        }

        Destroy(gameObject);
    }
}