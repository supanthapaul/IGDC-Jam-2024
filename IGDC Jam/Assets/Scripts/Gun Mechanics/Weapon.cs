using Health_System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool isBot;
    [SerializeField] protected Transform muzzle; //spawn point for bullets, and/or start point of raycasts if we're going for realism

    public GunStats stats;
    public BulletStats bullet;
    public LayerMask targets;
    public int currentAmmo;

    //firing stats
    protected bool isFiring;
    protected bool canFire; //currentAmmo>zero && not reloading && over the fire rate limits
                            //-- isFiring has the final call on whether a gun can fire or not

    protected bool isReloading; //cannot fire; limited by reloading
    protected bool isInFireRateWait; //cannot fire; limited by fireRate
    protected bool isEmpty; //cannot fire; no ammo, not reloading


    private WaitForSeconds reloadWait, fireRateWait;
    private Camera cam;

    private Vector3 cameraMidpoint = new(0.5f, 0.5f, 0f);
    public bool IsReloading { get => isReloading; } //getter fior external value read
    private Ray shootRay;

    protected virtual void SetUpStats()
    {
        canFire = currentAmmo > 0;
        reloadWait = new WaitForSeconds(stats.reloadTime);
        fireRateWait = new WaitForSeconds(1f / stats.fireRate);
        cam = Camera.main;
        Debug.Log((Camera.main == null) + " " + (cam == null));
    }

    //publicized for external calls
    public virtual void WeaponLogic()
    {
        CanFireCheck();
        ReadInputs();
    }

    protected virtual void ReadInputs()
    {
        if (Input.GetKeyDown(KeyCode.R)) //reload
        {
            StartCoroutine(Reload());
        }
    }

    protected virtual IEnumerator Reload()
    {
        isReloading = true;
        yield return reloadWait;
        currentAmmo = stats.maxCapacity;
        isEmpty = false;
        isReloading = false;

        //Alternative implementation
        //add reload cancel to the wait by changing
        //how we count down the timer (using "while" basically)
        //and not adding any more bullets if reload gets cancelled
        //before the countdown ends
    }

    //these limiters can be swapped with simple Time checkers where we check
    //if the (last fired time - current time) > fireRate
    //in Update() though
    //this I feel is more readable
    protected virtual IEnumerator FireRateLimiter()
    {
        isInFireRateWait = true;
        yield return fireRateWait;
        isInFireRateWait = false;
    }

    protected virtual void CanFireCheck()
    {
        canFire = !isInFireRateWait && !isReloading && !isEmpty;
    }
    protected virtual void Fire()
    {
        if(!isBot)
        {
            currentAmmo--;
        }
        switch (bullet.bulletType)
        {
            case BulletType.RAYCAST:
                Debug.Log(cam == null);
                if(!isBot)
                {
                    shootRay = cam.ViewportPointToRay(cameraMidpoint);
                }
                else
                {

                }
                float maxLineLength = 5f;
                if(Physics.SphereCast(shootRay, 0.25f, out RaycastHit hit, 150f, targets))
                {
                    if(hit.collider.TryGetComponent(out IHealth health))
                    {
                        health.TakeDamage(bullet.damage);
                    }
                    Instantiate(bullet.bulletObject.onHitEffect, hit.point, Quaternion.identity);
                    maxLineLength = Mathf.Min(hit.distance, maxLineLength);
                }

                //draw line till maxLineLength
                // use muzzle transform as origin and forward * maxlineLength for endPoint
                
                
                break;
            case BulletType.TRAVEL:
                Bullet currentBullet = Instantiate(bullet.bulletObject, muzzle.position, muzzle.rotation, null);
                currentBullet.Init(bullet.speed, bullet.damage);
                currentBullet.gameObject.tag = gameObject.tag;
                break;
            case BulletType.PHYSICAL:
                break;
            default:
                break;
        }

        if (currentAmmo == 0)
            isEmpty = true;
    }

    protected virtual void DisableWeapon()
    {
        StopAllCoroutines();
        isInFireRateWait = false;
        isReloading = false;
    }

    private void OnDisable()
    {
        DisableWeapon();
    }

}
