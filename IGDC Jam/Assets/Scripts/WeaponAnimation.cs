using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    [SerializeField] private WeaponsHolder weaponInventory;
    [SerializeField] private Weapon weaponToAnimate;
    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool hasReloaded;
    
    void Update()
    {
        if(!weaponToAnimate.IsReloading) 
            hasReloaded = false;

        if(weaponToAnimate.IsFiring)
        {
            if (!weaponToAnimate.IsInFireRateWait)
                animator.SetTrigger("Fire");
        }

        if(weaponToAnimate.IsReloading&&!hasReloaded)
        {
            animator.SetTrigger("Reload");
            hasReloaded = true;
        }

        if(Input.GetKey(KeyCode.L))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
