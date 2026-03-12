using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ActorController : MonoBehaviour
{
    public float health = 100;
    public bool isPlayer;
    public NavMeshAgent agent; // Optional for flying units
    
    [Header("Combat")]
    public GameObject projectilePrefab;
    public Transform shootPoint;

    public void TakeDamage(float amount)
    {
        health -= amount;

        // Add this part to trigger the audio
        if (isPlayer && TryGetComponent(out PlayerAudioController audioCtrl))
        {
            audioCtrl.PlayHurtClip();
        }

        if (health <= 0) Die();
    }

    private void Die()
    {
        if (isPlayer) GameCore.Instance.ChangeScene("LoseScreen");
        else
        {
            GameCore.Instance.RegisterKill(100, "A");
            Destroy(gameObject);
        }
    }

    public void Attack(Transform target)
    {
        var proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        proj.GetComponent<Rigidbody>().linearVelocity = (target.position - shootPoint.position).normalized * 20f;
    }
}