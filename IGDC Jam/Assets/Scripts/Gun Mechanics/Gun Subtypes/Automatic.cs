using UnityEngine;

public class Automatic : Weapon, IOnFireInputPressed, IOnFireInputReleased
{
    private void Start()
    {
        base.SetUpStats();
        stats.triggerType = TriggerType.AUTOMATIC;
    }


    public override void WeaponLogic()
    {
        CanFireCheck();
        ReadInputs();
    }

    protected override void ReadInputs()
    {
        base.ReadInputs();

        if (isReloading)
            return;

        if (Input.GetMouseButton(0))
            OnFireInputPressed();

        else if (Input.GetMouseButtonUp(0))
            OnFireInputReleased();
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
}
