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
    private bool wasDisabled = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = weaponToAnimate.enabled;
    }


    void Update()
    {
        if(!weaponToAnimate.IsReloading)
            hasReloaded = false;
        
        if(wasDisabled && weaponToAnimate.enabled)
        {
            animator.SetTrigger("Equip");
            spriteRenderer.enabled = true; 
            wasDisabled = false;
        }

        if(!wasDisabled && !weaponToAnimate.enabled)
        {
            spriteRenderer.enabled = false; 
            wasDisabled = true;
        }

        animator.SetBool("Firing", weaponToAnimate.IsFiring);

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
