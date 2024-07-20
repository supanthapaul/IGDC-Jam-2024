using UnityEngine;

public class Automatic : Weapon, IOnFireInputPressed, IOnFireInputReleased, IOnReloadStart
{
    private void Start()
    {
        base.SetUpStats();
        GetInterfaces();
        stats.triggerType = TriggerType.AUTOMATIC;
    }


    public override void WeaponLogic()
    {
        CanFireCheck();
    }

    public void OnFireInputPressed()
    {
        if (!canFire)
            return;

        Fire();
        StartCoroutine(FireRateLimiter());
    }

    public void OnFireInputReleased()
    {
        isFiring = false;
    }

    public void OnReloadPressed()
    {
        StartCoroutine(ReloadCoroutine());
    }

    protected override void GetInterfaces()
    {
        onFireContinuous = GetComponent<IOnFireInputPressed>();
        onFireReleased = GetComponent<IOnFireInputReleased>();
        onReload = GetComponent<IOnReloadStart>();
    }
}
