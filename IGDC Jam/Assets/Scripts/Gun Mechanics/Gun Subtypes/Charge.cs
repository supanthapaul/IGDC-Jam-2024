using System;
using UnityEngine;

//Charge Weapon class
//There are many implementations of a charge weapons in games
//I am implementing a version in which the player has to charge up the
//weapon for a minimum "x" seconds before it fires.
//Players can hold the charge for "y" seconds after it reaches maximum charge (1f, always),
//but they cannot discharge the weapon before the minimum charge threshold is met
//moreover, there is an cooldown time after the weapon discharges

//a recent example would be the railgun from Helldivers 2  

//I have considered other variants of implementations and I feel that
//they are equally achieavable with this architecture
public class Charge : Weapon, IOnFireInputStart, IOnFireInputReleased
{
    [SerializeField] private bool isCharging, hasCooledDown;
    [SerializeField] private float chargeRate, chargeDecay;

    [SerializeField] private float currentCharge;
    [SerializeField, Range(0.01f, 0.99f)] private float minCharge;

    [SerializeField] private float maxChargeHoldTime;
    [SerializeField] private float cooldownTime;

    private float currentChargeHoldTime;
    private float currentCooldownTime;

    public float CurrentCharge { get => currentCharge; private set => currentCharge = value; }

    void Start()
    {
        SetUpStats();
        currentCharge = 0f;
        stats.triggerType = TriggerType.CHARGE;
        hasCooledDown = true;
    }

    //void Update()
    //{
    //    CanFireCheck();
    //    ReadInputs();
    //}

    public override void WeaponLogic()
    {
        CanFireCheck();
        ReadInputs();
        ChargeCalculations();
    }

    protected override void CanFireCheck()
    {
        base.CanFireCheck();
        canFire &= currentCooldownTime <= 0f;
    }

    protected override void ReadInputs()
    {
        base.ReadInputs();
        if (isReloading) return;

        if (Input.GetMouseButtonDown(0) && canFire)
        {
            OnFireInputStart();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnFireInputReleased();
        }
    }
    private void ChargeCalculations()
    {
        //let weapon cooldown
        if (currentCooldownTime >= 0f)
        {
            currentCooldownTime -= Time.deltaTime;
            
            return;
        }

        if(!hasCooledDown )
        {
            currentCharge -= chargeDecay * Time.deltaTime;
            if (currentCharge < 0f)
            {
                currentCharge = 0f;
                hasCooledDown = true;
            }
            return;
        }

        //add charge according to isCharging, and clamp to keep it normalized
        currentCharge += (isCharging ? chargeRate : -chargeDecay) * Time.deltaTime;
        currentCharge = Mathf.Clamp01(currentCharge);

        //if the weapon trigger has been held for too long, fire (if it can)
        if (canFire && currentChargeHoldTime > maxChargeHoldTime)
        {
            Fire();
        }

        //start the maxChargeHold timer
        if (currentCharge == 1f)
        {
            currentChargeHoldTime += Time.deltaTime;
        }
    }

    public void OnFireInputStart()
    {
        isCharging = true;
    }

    public void OnFireInputReleased()
    {
        if (currentCharge > minCharge)
        {
            Fire();
        }
        isCharging = false;
    }

    protected override void Fire()
    {
        base.Fire();
        currentCooldownTime = cooldownTime;
        currentChargeHoldTime = 0f;
        isCharging = false;
        hasCooledDown = false;
    }

    private void OnDisable()
    {
        base.DisableWeapon();

        isCharging = false;
        currentCharge = 0f;
        currentChargeHoldTime = 0;
        currentCooldownTime = 0f;
        hasCooledDown = true;
    }
}
