using UnityEngine;

//Semi Auto Weapon inheriting Weapon class
//The implementation goes as follows
//We capture first frame of FireInput to fire
//and then wait for a triggerReset on the event of FireInput Released
public class SemiAuto : Weapon, IOnFireInputStart, IOnFireInputReleased
{
    private bool triggerReset;
    // Start is called before the first frame update
    void Start()
    {
        SetUpStats();
        stats.triggerType = TriggerType.SEMIAUTO;
    }

    protected override void SetUpStats()
    {
        base.SetUpStats();
        triggerReset = true;
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

        if (Input.GetMouseButtonDown(0))
        {
            OnFireInputStart();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnFireInputReleased();
        }
    }

    protected override void CanFireCheck()
    {
        base.CanFireCheck();
        canFire &= triggerReset; //canFire still holds final authority over whether weapon can fire or not
    }

    public void OnFireInputStart()
    {
        if (!canFire)
            return;

        Fire();
        triggerReset = false;
        StartCoroutine(FireRateLimiter());
    }
    public void OnFireInputReleased()
    {
        triggerReset = true;
    }
}
