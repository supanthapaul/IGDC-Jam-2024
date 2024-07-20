using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHolder : MonoBehaviour
{
    public List<Weapon> weaponList;
    public int equippedWeaponIndex = -1; // 0 for first primary, 1 for second primary,
                                         // 2 for secondary, -1 for nothing equipped
    public Weapon currentEquippedWeapon;

    public delegate void OnWeaponChange(int weaponIndex);
    public event OnWeaponChange WeaponChange;

    private void Start()
    {
        weaponList.Capacity = Mathf.Min(3, weaponList.Count);
        weaponList[0].enabled = false;
        weaponList[1%weaponList.Count].enabled = false;
        weaponList[2%weaponList.Count].enabled = false;
    }

    private void Update()
    {
        SwitchWeaponsOnInput();
        if(currentEquippedWeapon!=null)
            CurrentWeaponLogic();
    }

    private void CurrentWeaponLogic()
    {
        currentEquippedWeapon.WeaponLogic();
    }

    private void SwitchWeaponsOnInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetActiveWeapon(2);
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
        WeaponChange?.Invoke(equippedWeaponIndex);
        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponList[i].enabled = i == weaponIndex;
        }
        //Debug.Log(weaponIndex + " " + currentEquippedWeapon != null ? currentEquippedWeapon.name : "Unequipped");
    }
}
