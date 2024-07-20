using UnityEngine;

//Semi Auto Weapon inheriting Weapon class
//The implementation goes as follows
//We capture first frame of FireInput to fire
//and then wait for a triggerReset on the event of FireInput Released
public class SemiAuto : Weapon, IOnFireInputStart, IOnFireInputReleased, IOnReloadStart
{
    private bool triggerReset;
    // Start is called before the first frame update
    void Start()
    {
        GetInterfaces();
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

    protected override void GetInterfaces()
    {
        onFireStart = GetComponent<IOnFireInputStart>();
        onFireReleased = GetComponent<IOnFireInputReleased>();
        onReload = GetComponent<IOnReloadStart>();
    }

    public void OnReloadPressed()
    {
        StartCoroutine(ReloadCoroutine());
    }
}
