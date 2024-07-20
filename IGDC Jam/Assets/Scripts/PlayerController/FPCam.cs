using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerPrefStatics;

public class FPCam : AbilityUpdate
{
    public bool hasHorizontalLook;
    public bool hasVerticalLook;

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    public Transform orientation;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetUpRestrictions();
    }

    [ContextMenu("Take Away Look Abilities")]
    private void TakeAbilities()
    {
        PlayerPrefs.SetInt(LookHorizontalRestriction, 0);
        PlayerPrefs.SetInt(LookVerticalRestriction, 0);
        SetUpRestrictions();
    }

    [ContextMenu("Give Look Abilities")]
    private void GiveAbilities()
    {
        PlayerPrefs.SetInt(LookHorizontalRestriction, 1);
        PlayerPrefs.SetInt(LookVerticalRestriction, 1);
        SetUpRestrictions();
    }


    public override void SetUpRestrictions()
    {
        hasHorizontalLook = PlayerPrefs.GetInt(LookHorizontalRestriction, 0) == 1;
        hasVerticalLook = PlayerPrefs.GetInt(LookVerticalRestriction, 0) == 1;
    }

    private void Update()
    {
        float mouseX = 0, mouseY = 0;
        if(hasHorizontalLook)
            mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;

        if(hasVerticalLook)
            mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        
    }
}
