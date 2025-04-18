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

    [Header("UI")]
    public Slider chargeSlider;

    private float chargeStartTime;
    private bool isCharging;
    private float lastFireTime;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            chargeStartTime = Time.time;
            isCharging = true;
            if (animatorController != null) animatorController.isThrowing = true;
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
                    fillImage.color = Color.Lerp(Color.white, Color.red, chargePercent);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isCharging && Time.time >= lastFireTime + fireCooldown)
        {
            float chargeTime = Mathf.Clamp(Time.time - chargeStartTime, 0, maxChargeTime);
            float chargePercent = chargeTime / maxChargeTime;
            float finalSpeed = Mathf.Lerp(10f, maxProjectileSpeed, chargePercent);

            Shoot(finalSpeed, chargePercent);

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

            // Reset isThrowing a bit later to allow animation to play
            if (animatorController != null) StartCoroutine(ResetThrowing(0.3f));
        }
    }
    private IEnumerator ResetThrowing(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (animatorController != null)
            animatorController.isThrowing = false;
    }

    // Changed to account for the player's velocity as well as the pre-defined and charge velocity (prevents player moving faster than the gun can shoot)
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