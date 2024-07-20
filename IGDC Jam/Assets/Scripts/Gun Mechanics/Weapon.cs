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

    public IOnFireInputStart onFireStart;
    public IOnFireInputPressed onFireContinuous;
    public IOnFireInputReleased onFireReleased;
    public IOnReloadStart onReload;

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
    public bool IsReloading { get => isReloading; } //getter for external value read
    public bool IsFiring { get => isFiring;}
    public bool IsInFireRateWait { get => isInFireRateWait; }

    private Ray shootRay;

    protected virtual void SetUpStats()
    {
        canFire = currentAmmo > 0;
        reloadWait = new WaitForSeconds(stats.reloadTime);
        fireRateWait = new WaitForSeconds(1f / stats.fireRate);
        cam = Camera.main;
    }

    //publicized for external calls
    public virtual void WeaponLogic()
    {
        CanFireCheck();
    }

    protected virtual IEnumerator ReloadCoroutine()
    {
        //already reloading, don't do shit
        if (isReloading) yield break;

        isReloading = true;
        yield return reloadWait;
        currentAmmo = stats.maxCapacity;
        isEmpty = false;
        isReloading = false;
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
                shootRay = cam.ViewportPointToRay(cameraMidpoint);
                
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
        {
            isEmpty = true;
            isFiring = false;
        }
    }

    protected virtual void DisableWeapon()
    {
        StopAllCoroutines();
        isInFireRateWait = false;
        isReloading = false;
    }
    protected abstract void GetInterfaces();

    private void OnDisable()
    {
        DisableWeapon();
    }

    private void OnEnable()
    {
        if(muzzle == null)
        {
            muzzle = transform.GetChild(0);
        }
    }

}
