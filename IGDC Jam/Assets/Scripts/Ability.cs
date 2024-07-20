using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerPrefStatics;

public enum Abilities
{
    ForwardWalk,
    HorizontalLook,
    VerticalLook,
    Strafe,
    Jump,
    Dash,
    Wallrun,
    Crouch,
    Slide,
    Fire,
    Reload
}
public class Ability : MonoBehaviour
{
    public Abilities nextUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GiveAbility(nextUnlock);
        }
  
        GameManager.Instance.SetAllRestrictions();
        
        Destroy(gameObject);
    }

}
