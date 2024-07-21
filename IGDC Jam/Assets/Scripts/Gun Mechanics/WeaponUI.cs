using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private WeaponsHolder weaponHolder; //would've used a service locator or a instance ideally
    [SerializeField] private TextMeshProUGUI ammoCountField;
    [SerializeField] private GameObject weaponUiObj;

    private void Update()
    {
        if (weaponHolder.currentEquippedWeapon == null)
        {
            weaponUiObj.SetActive(false);
            return;
        }
        weaponUiObj.SetActive(true);
        ammoCountField.text = weaponHolder.currentEquippedWeapon.currentAmmo + "/" + weaponHolder.currentEquippedWeapon.stats.maxCapacity;
    }
}
