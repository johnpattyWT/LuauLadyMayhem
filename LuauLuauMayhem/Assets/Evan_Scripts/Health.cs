using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    public Image Background;

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
        // One-time hit detection (e.g., projectiles)
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Hit by Projectile! Taking damage.");
            StartCoroutine(FlashRed());
            TakeDamage();
        }
    }

    private float damageCooldown = 2f; // Time in seconds between damage ticks
    private float lastDamageTime = -Mathf.Infinity;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Deathzone"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                Debug.Log("Still in Deathzone! Taking continuous damage.");
                StartCoroutine(FlashRed());
                TakeDamage();
                lastDamageTime = Time.time;
            }
        }
    }

    public IEnumerator FlashRed()
    {
        Background.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        Background.color = Color.white;
    }
}
