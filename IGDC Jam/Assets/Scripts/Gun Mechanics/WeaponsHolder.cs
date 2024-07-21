using System.Collections.Generic;
using UnityEngine;
using static PlayerPrefStatics;

public class WeaponsHolder : AbilityUpdate
{
    public bool hasFire;
    public bool hasReload;
    public List<Weapon> weaponList;
    public int equippedWeaponIndex = -1; // 0 for first primary, 1 for second primary,
                                         // 2 for secondary, -1 for nothing equipped
    public Weapon currentEquippedWeapon;


    private void Start()
    {
        SetUpRestrictions();

    }

    private void Update()
    {
        if (weaponList.Count == 0) return; 
        if (!hasFire) return;
        SwitchWeaponsOnInput();
        if(currentEquippedWeapon!=null)
            CurrentWeaponLogic();
    }

    private void CurrentWeaponLogic()
    {
        currentEquippedWeapon.WeaponLogic();

        if (currentEquippedWeapon.IsReloading) return;


        if(Input.GetMouseButtonDown(0))
        {
            currentEquippedWeapon.onFireStart?.OnFireInputStart();
        }
        if (Input.GetMouseButton(0))
        {
            currentEquippedWeapon.onFireContinuous?.OnFireInputPressed();
        }
        if (Input.GetMouseButtonUp(0))
        {
            currentEquippedWeapon.onFireReleased?.OnFireInputReleased();
        }

        if(!hasReload) return;  

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentEquippedWeapon.onReload?.OnReloadPressed();
        }
    }

    private void SwitchWeaponsOnInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetActiveWeapon(-1);
        }
    }

    public void SetActiveWeapon(int weaponIndex)
    {
        if (weaponIndex == equippedWeaponIndex) return; //no need to change anything

        equippedWeaponIndex = weaponIndex;
        if (weaponIndex == -1)
        {
            foreach (Weapon weapon in weaponList)
            {
                weapon.enabled = false;
            }
            currentEquippedWeapon = null;
            return;
        }
        currentEquippedWeapon = weaponList[weaponIndex];
        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponList[i].enabled = i == weaponIndex;
        }
    }

    public void AddWeapon()
    {
        hasFire = true;
        SetActiveWeapon(0);
        GameManager.Instance.GiveAbility(Abilities.Fire);
    }

    public override void SetUpRestrictions()
    {
        hasFire = PlayerPrefs.GetInt(FireRestriction, 0) == 1;
        hasReload = PlayerPrefs.GetInt(ReloadRestriction, 0) == 1;

        SetActiveWeapon(hasFire?0:-1);
    }

    [ContextMenu("Take Away All Abilities")]
    public void TakeAwayAbilities()
    {
        PlayerPrefs.SetInt(FireRestriction, 0);
        PlayerPrefs.SetInt(ReloadRestriction, 0);
        SetUpRestrictions();
    }
    [ContextMenu("Give All Abilities")]
    public void GiveAllAbilities()
    {
        PlayerPrefs.SetInt(FireRestriction, 1);
        PlayerPrefs.SetInt(ReloadRestriction, 1);
        SetUpRestrictions();
    }
}
