using Invector.vCharacterController;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float maxChargeTime = 4f;
    public float maxProjectileSpeed = 50f;
    public float fireCooldown = 0.25f;
    public Camera playerCamera;
    private Animator animator;
    private vThirdPersonController controller;

    [Header("UI")]
    public Slider chargeSlider;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip chargeStartClip;
    public AudioClip chargingLoopClip;
    public AudioClip shootClip;
    public AudioClip chargeCancelClip;

    private float chargeStartTime;
    private bool isCharging;
    private float lastFireTime;

    private AudioSource chargingLoopSource;

    private void Start()
    {


        controller = GetComponent<vThirdPersonController>();
        animator = GetComponent<Animator>();

        if (chargingLoopClip != null)
        {
            chargingLoopSource = gameObject.AddComponent<AudioSource>();
            chargingLoopSource.clip = chargingLoopClip;
            chargingLoopSource.loop = true;
            chargingLoopSource.playOnAwake = false;
        }
    }
   
    void Update()
    {
     
            if (Time.timeScale == 0f)
                return;

         

        if (Input.GetMouseButtonDown(0))
        {
            chargeStartTime = Time.time;
            isCharging = true;
            if (animatorController != null) animatorController.isThrowing = true;

            // Play charge start sound
            if (audioSource != null && chargeStartClip != null)
                audioSource.PlayOneShot(chargeStartClip);

            // Start charging loop sound
            if (chargingLoopSource != null)
                chargingLoopSource.Play();
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            float chargeTime = Mathf.Clamp(Time.time - chargeStartTime, 0, maxChargeTime);
            float chargePercent = chargeTime / maxChargeTime;

            if (chargeSlider != null)
            {
                chargeSlider.value = chargePercent;
                if (chargeSlider.fillRect != null)
                {
                    Image fillImage = chargeSlider.fillRect.GetComponent<Image>();
                    fillImage.color = Color.Lerp(Color.yellow, Color.red, chargePercent);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isCharging && Time.time >= lastFireTime + fireCooldown)
        {
            float chargeTime = Mathf.Clamp(Time.time - chargeStartTime, 0, maxChargeTime);
            float chargePercent = chargeTime / maxChargeTime;
            float finalSpeed = Mathf.Lerp(10f, maxProjectileSpeed, chargePercent);

            Shoot(finalSpeed, chargePercent);
            animator.SetTrigger("isThrowing");

            if (chargeSlider != null)
            {
                chargeSlider.value = 0;
                if (chargeSlider.fillRect != null)
                {
                    Image fillImage = chargeSlider.fillRect.GetComponent<Image>();
                    fillImage.color = Color.white;
                }
            }

            lastFireTime = Time.time;
            isCharging = false;

            // Stop charging loop
            if (chargingLoopSource != null)
                chargingLoopSource.Stop();

            // Play shooting sound
            if (audioSource != null && shootClip != null)
                audioSource.PlayOneShot(shootClip);

            // Reset isThrowing a bit later to allow animation to play
            if (animatorController != null) StartCoroutine(ResetThrowing(0.3f));
        }

        // Optional: cancel charging if mouse is released too soon
        if (Input.GetMouseButtonUp(0) && isCharging && Time.time < lastFireTime + fireCooldown)
        {
            isCharging = false;
            if (chargingLoopSource != null)
                chargingLoopSource.Stop();

            if (audioSource != null && chargeCancelClip != null)
                audioSource.PlayOneShot(chargeCancelClip);
        }
    }

    private IEnumerator ResetThrowing(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (animatorController != null)
            animatorController.isThrowing = false;
    }

    void Shoot(float speed, float chargePercent)
    {
        if (projectilePrefab == null || shootPoint == null || playerCamera == null)
            return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Vector3 shootDirection = ray.direction;

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(shootDirection));
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Vector3 playerVelocity = Vector3.zero;
        Rigidbody playerRb = GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerVelocity = playerRb.linearVelocity;
        }

        if (rb != null)
        {
            rb.linearVelocity = shootDirection * speed + playerVelocity;
        }

        float minExplosionForce = 500f;
        float maxExplosionForce = 1200f;
        float explosionForce = Mathf.Lerp(minExplosionForce, maxExplosionForce, chargePercent);

        ExplosionScript explosionScript = projectile.GetComponent<ExplosionScript>();
        if (explosionScript != null)
        {
            explosionScript.SetExplosionForce(explosionForce);
            explosionScript.SetExplosionScale(chargePercent);
        }
    }

    private vThirdPersonAnimator animatorController;

    private void Awake()
    {
        animatorController = GetComponent<vThirdPersonAnimator>();
    }
}
