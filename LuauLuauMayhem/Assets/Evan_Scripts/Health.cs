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

    private void Start()
    {
        currentHealth = maxHealth;

        // Immediately set the slider and fill color at start
        if (healthSlider != null)
            healthSlider.value = 1f;
        if (healthFillImage != null)
            healthFillImage.color = fullHealthColor;
    }

    private void Update()
    {
        // Smoothly update the slider value toward the target value
        float targetValue = (float)currentHealth / maxHealth;
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetValue, Time.deltaTime * smoothSpeed);
        }

        // Update the color from low health (red) to full health (green)
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
        // Check for a collision with objects tagged as "Projectile"
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Hit by Projectile! Taking damage.");
            TakeDamage();
        }
    }
}