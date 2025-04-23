using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("UI Elements")]
    public Slider healthSlider;          // Assign your Slider in the Inspector
    public Image healthFillImage;        // Assign the fill Image of your Slider

    [Header("Health Colors")]
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;

    [Header("Smooth Transition Settings")]
    [Tooltip("Speed at which the health bar slides to the new value.")]
    public float smoothSpeed = 3f;

    private PlayerAudioController audioController;

    private void Start()
    {
        currentHealth = maxHealth;

        audioController = GetComponent<PlayerAudioController>();

        if (healthSlider != null)
            healthSlider.value = 1f;
        if (healthFillImage != null)
            healthFillImage.color = fullHealthColor;
    }

    private void Update()
    {
        float targetValue = (float)currentHealth / maxHealth;
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetValue, Time.deltaTime * smoothSpeed);
        }

        if (healthFillImage != null)
        {
            healthFillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, targetValue);
        }
    }

    /// <summary>
    /// Call this function to apply damage to the player.
    /// Each hit decreases health by 20.
    /// </summary>
    public void TakeDamage()
    {
        currentHealth -= 20;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (audioController != null)
        {
            audioController.PlayHurtClip();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Called when the player's health reaches 0.
    /// </summary>
    private void Die()
    {
        Debug.Log("Player Died!");
        SceneManager.LoadScene("LoseScreen");
    }

    private void OnCollisionEnter(Collision collision)
    {
<<<<<<< HEAD
=======
        // One-time hit detection (for projectiles, etc.)
>>>>>>> 0e3eb29e9b459b247abfe221f599848dbf6218ed
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Hit by Projectile! Taking damage.");
            TakeDamage();
        }
    }
<<<<<<< HEAD
}
=======

    private void OnCollisionStay(Collision collision)
    {
        // Continuous damage when staying in the Deathzone
        if (collision.gameObject.CompareTag("Deathzone"))
        {
            Debug.Log("Still in Deathzone! Taking continuous damage.");
            TakeDamage();
        }
    }

}
>>>>>>> 0e3eb29e9b459b247abfe221f599848dbf6218ed
