using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Camera playerCamera;
    public Slider chargeSlider;

    private float _chargeTime;
    private bool _isCharging;

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetMouseButtonDown(0)) _isCharging = true;

        if (_isCharging)
        {
            _chargeTime = Mathf.Min(_chargeTime + Time.deltaTime, 2f);
            if (chargeSlider) chargeSlider.value = _chargeTime / 2f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Fire();
            _isCharging = false;
            _chargeTime = 0;
            if (chargeSlider) chargeSlider.value = 0;
        }
    }

    void Fire()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(ray.direction));
        
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.linearVelocity = ray.direction * (20f * (1 + _chargeTime));
    }
}