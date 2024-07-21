using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private WeaponsHolder weaponHolder; //would've used a service locator or a instance ideally
    [SerializeField] private TextMeshProUGUI ammoCountField;
    [SerializeField] private GameObject reloadTextObject;

    private void Update()
    {
        if (weaponHolder.currentEquippedWeapon == null) return;

        ammoCountField.text = weaponHolder.currentEquippedWeapon.currentAmmo + "/" + weaponHolder.currentEquippedWeapon.stats.maxCapacity;
        reloadTextObject.SetActive(weaponHolder.currentEquippedWeapon.IsReloading);
    }
}
