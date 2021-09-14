using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private bool isAutomatic = false;
    [SerializeField] private bool infiniteAmmo = false;
    [SerializeField] private int ammo = 30;
    [SerializeField] private float cooldownTime = 0.2f;
    [SerializeField] private Transform muzzlePoint = null;
    [SerializeField] private UnityEvent OnSuccesfullShot;
    [SerializeField] private UnityEvent OnEveryShot;
    [SerializeField] private float bulletDamage = 10f;
    [SerializeField] private UnityEvent OnStartShoot;
    [SerializeField] private UnityEvent OnStopShoot;

    [Header("Effects")]
    [SerializeField] private bool continuesMuzzleFlash = false;
    [SerializeField] private ParticleSystem muzzleFlash = null;
    [SerializeField] private AudioClip shootSound = null;
    [SerializeField] private GameObject impactEffect = null;
    [SerializeField] private Light muzzleLight = null;
    [SerializeField] private float muzzleLightTime = 0.05f;

    [Header("Screen shake")]
    [SerializeField] private bool useScreenShake = true;
    [SerializeField] private float shakeSpeed = 2f;
    [SerializeField] private float shakeDuration = 0.25f;
    [SerializeField] private float shakeDistance = 0.05f;

    private ScreenShake screenShake;

    [Header("Recoil")]
    [SerializeField] private bool recoil = true;
    [SerializeField] private Vector3 recoilTorque = Vector3.right;
    [SerializeField] private Vector3 recoilForce = Vector3.forward;
    [Space(10)]
    [SerializeField] private float recoilTorqueSmoothing = 0.95f;
    [SerializeField] private float recoilForceSmoothing = 0.95f;
    [Space(10)]
    [SerializeField] private bool randomizeXRotation = false;
    [SerializeField] private bool randomizeYRotation = false;
    [SerializeField] private bool randomizeZRotation = true;

    [Header("Projectile settings")]
    [SerializeField] private GameObject projectileObject = null;
    [SerializeField] private Vector3 projectileForce = Vector3.forward;
    [SerializeField] private float projectileDespawnTime = 15f;

    [Header("Raycast settings")]
    [SerializeField] private LayerMask playerLayer = 0;
    [SerializeField] private float maxHitDistance = 100f;
    [SerializeField] private float bulletDespawnTime = 10f;
    [SerializeField] private float bulletImpactForce = 2f;
    [SerializeField] private float bulletWidth = 0.05f;

    [SerializeField] private Animator animator = null;
    [SerializeField] private string animationTrigger = "Open";

    private float timeBeforeNextShot = 0f;
    private AudioSource audioSource = null;
    private Vector3 currentRecoilRotation = Vector3.zero;
    private Vector3 currentRecoilPosition = Vector3.zero;
    private Quaternion startRotation = Quaternion.identity;
    private Vector3 startPosition = Vector3.zero;
    private bool isInUse = true;

    private float targetAudioVolume = 1f;
    private float targetLightBrightness = 1f;

    private Camera mainCamera = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timeBeforeNextShot = Time.time - cooldownTime;
        startRotation = transform.localRotation;
        startPosition = transform.localPosition;
        mainCamera = Camera.main;
        screenShake = ScreenShake.Instance;
        if (muzzleFlash != null && muzzleFlash.isPlaying) muzzleFlash.Stop();
        if (muzzleLight != null) targetLightBrightness = muzzleLight.intensity;
        targetAudioVolume = audioSource.volume;
    }

    public void Update()
    {
        if (UIManager.isPaused)
        {
            timeBeforeNextShot = Time.time - cooldownTime;
            return;
        }
        HandleInput();
    }

    public void LateUpdate()
    {
        if (!isInUse || !recoil) return;
        ApplyRecoil();
    }

    /// <summary>
    /// Listens to input and performs Shoot method when held in hand
    /// </summary>
    public virtual void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && isInUse)
        {
            if (Time.frameCount < 5) return;
            Shoot();
            if (continuesMuzzleFlash)
            {
                if (muzzleFlash != null) muzzleFlash.Play();
                if (muzzleLight != null)
                {
                    muzzleLight.gameObject.SetActive(true);
                    muzzleLight.DOIntensity(targetLightBrightness, 0.3f);
                }
                if (audioSource != null)
                {
                    audioSource.Play();
                    audioSource.DOFade(targetAudioVolume, 0.3f);
                }
                if(animator != null) animator.SetBool(animationTrigger, true);
                OnStartShoot.Invoke();
            }
        }
        else if (Input.GetMouseButtonUp(0) && isInUse)
        {
            if (continuesMuzzleFlash)
            {
                if (muzzleFlash != null) muzzleFlash.Stop();
                if (muzzleLight != null) muzzleLight.DOIntensity(0f, 0.3f);
                if (audioSource != null) audioSource.DOFade(0f, 0.3f);
                if (animator != null) animator.SetBool(animationTrigger, false);
                OnStopShoot.Invoke();
            }
        }

        if (isAutomatic && Input.GetMouseButton(0) && isInUse)
        {
            Shoot();
        }
    }

    /// <summary>
    /// When time allows to shoot, shoots and retrieves info while also instatiating an impact effect and acting according to hit object with pushback
    /// </summary>
    public virtual void Shoot()
    {
        if (Time.time > timeBeforeNextShot)
        {
            if (ammo <= 0) return;
            if (!infiniteAmmo) ammo--;

            timeBeforeNextShot = Time.time + cooldownTime;

            if (projectileObject == null)
            {
                if (Physics.SphereCast(muzzlePoint.transform.position, bulletWidth, muzzlePoint.transform.forward, out RaycastHit hit, maxHitDistance, ~playerLayer))
                {
                    if (hit.collider != null)
                    {
                        if ((hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Damageable")) && hit.transform.gameObject.TryGetComponent(out EntityHealth entityHealth))
                        {
                            entityHealth.DealDamage(bulletDamage);
                        }
                        else if (impactEffect != null)
                        {
                            GameObject hitImpactEffect = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                            Destroy(hitImpactEffect, bulletDespawnTime);
                        }

                        Rigidbody hitRb = null;
                        if (hit.transform.TryGetComponent(out hitRb) || hit.transform.root.TryGetComponent(out hitRb))
                        {
                            hitRb.AddForceAtPosition(-hit.normal * bulletImpactForce, hit.point, ForceMode.Impulse);
                        }
                    }

                    OnSuccesfullShot.Invoke();
                }
            }
            else
            {
                GameObject projectile = Instantiate(projectileObject, muzzlePoint.position, muzzlePoint.rotation);
                Rigidbody projectileRb;
                if (!projectile.TryGetComponent(out projectileRb))
                {
                    projectileRb = projectile.AddComponent<Rigidbody>();
                }

                projectileRb.AddForce(muzzlePoint.TransformDirection(projectileForce), ForceMode.Impulse);
                Destroy(projectile, projectileDespawnTime);
            }

            if (!continuesMuzzleFlash)
            {
                if (muzzleFlash != null) muzzleFlash.Play();
                if (shootSound != null) audioSource.PlayOneShot(shootSound);
                if (muzzleLight != null) StartCoroutine(FlickerMuzzleLight());
            }
            else
            {
                transform.DOShakePosition(cooldownTime, 0.01f, 10, 90, false, false);
            }

            if (useScreenShake && screenShake != null)
            {
                screenShake.Shake(shakeDuration, shakeDistance, Mathf.RoundToInt(shakeSpeed));
            }

            if (recoil)
            {
                Vector3 fixedRecoilTorque = recoilTorque;
                if (randomizeXRotation) fixedRecoilTorque.x *= Random.Range(0, 2) * 2 - 1;
                if (randomizeYRotation) fixedRecoilTorque.y *= Random.Range(0, 2) * 2 - 1;
                if (randomizeZRotation) fixedRecoilTorque.z *= Random.Range(0, 2) * 2 - 1;

                currentRecoilRotation += fixedRecoilTorque;
                currentRecoilPosition += recoilForce;
            }

            OnEveryShot.Invoke();
        }
    }

    /// <summary>
    /// Sets the start position and rotation to own current local position and rotation
    /// </summary>
    public void ResetRecoilPositionAndRotation()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    /// <summary>
    /// Calculates current recoil and applies it to the weapon
    /// </summary>
    public virtual void ApplyRecoil()
    {
        currentRecoilRotation *= Mathf.Pow(recoilForceSmoothing, Time.deltaTime);
        currentRecoilPosition *= Mathf.Pow(recoilTorqueSmoothing, Time.deltaTime);

        transform.localRotation = startRotation * Quaternion.Euler(currentRecoilRotation);
        transform.localPosition = startPosition + currentRecoilPosition;
    }

    /// <summary>
    /// Briefly turns light from muzzle flash on and off after given time
    /// </summary>
    /// <returns></returns>
    IEnumerator FlickerMuzzleLight()
    {
        if (muzzleLight != null)
        {
            muzzleLight.gameObject.SetActive(true);
            yield return new WaitForSeconds(muzzleLightTime);
            muzzleLight.gameObject.SetActive(false);
        }
    }
}
