using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WeaponUI : MonoBehaviour
{
    [SerializeField] private WeaponsHolder weaponHolder; //would've used a service locator or a instance ideally
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Toggle[] weaponToggles;
    [SerializeField] private Image chargeMeter;
    [SerializeField] private TextMeshProUGUI ammoCountField;
    [SerializeField] private GameObject reloadTextObject;

    private Charge castedChargeWeapon;
    private Weapon equippedWeapon;
    private GameObject chargeUIParent;
    private void Start()
    {
        weaponHolder.WeaponChange += OnWeaponChanged;

        equippedWeapon = weaponHolder.currentEquippedWeapon;
        if (!(weaponHolder.equippedWeaponIndex == -1))
        {
            weaponToggles[weaponHolder.equippedWeaponIndex].isOn = true;
        }
        foreach (Toggle weaponToggle in weaponToggles)
        {
            weaponToggle.onValueChanged.AddListener((_) => ChangeWeaponThroughToggle());
        }
        chargeUIParent = chargeMeter.transform.parent.gameObject;
    }

    private void Update()
    {
        if (equippedWeapon == null) return;

        if (equippedWeapon.stats.triggerType == TriggerType.CHARGE)
        {
            chargeMeter.fillAmount = castedChargeWeapon.CurrentCharge;
        }
        ammoCountField.text = equippedWeapon.currentAmmo + "/" + equippedWeapon.stats.maxCapacity;
        reloadTextObject.SetActive(equippedWeapon.IsReloading);
    }

    private void OnWeaponChanged(int weaponIndex)
    {
        equippedWeapon = weaponHolder.currentEquippedWeapon;
        if (equippedWeapon.stats.triggerType == TriggerType.CHARGE)
        {
            castedChargeWeapon = (Charge)equippedWeapon;

            chargeMeter.fillAmount = 0;
            chargeUIParent.SetActive(true);
        }
        else
        {
            chargeMeter.fillAmount = 0;
            chargeUIParent.SetActive(false);
        }
        for (int i = 0; i < weaponToggles.Length; i++)
        {
            weaponToggles[i].SetIsOnWithoutNotify(i == weaponIndex);
        }
    }

    private void ChangeWeaponThroughToggle()
    {
        RectTransform onToggle = null;
        foreach (Toggle t in weaponToggles)
        {
            if (t.isOn)
                onToggle = t.transform as RectTransform;
        }
        if (onToggle == null)
            weaponHolder.SetActiveWeapon(-1);
        else
            weaponHolder.SetActiveWeapon(onToggle.GetSiblingIndex());
    }
}
