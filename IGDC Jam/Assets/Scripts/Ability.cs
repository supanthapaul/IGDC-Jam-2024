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

    private AbilityUpdate[] abilities;
    private void Start()
    {
        abilities = FindObjectsByType<AbilityUpdate>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var ability in abilities)
        {
            ability.SetUpRestrictions();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GiveAbility(nextUnlock);
        }
        foreach (var ability in abilities)
        {
            ability.SetUpRestrictions();
        }
        Destroy(gameObject);
    }

    private void GiveAbility(Abilities nextUnlock)
    {
        switch (nextUnlock)
        {
            case Abilities.ForwardWalk:
                PlayerPrefs.SetInt(ForwardRestriction, 1);
                break;
            case Abilities.HorizontalLook:
                PlayerPrefs.SetInt(LookHorizontalRestriction, 1);
                break;
            case Abilities.VerticalLook:
                PlayerPrefs.SetInt(LookVerticalRestriction, 1);
                break;
            case Abilities.Strafe:
                PlayerPrefs.SetInt(StrafeRestriction, 1);
                break;
            case Abilities.Jump:
                PlayerPrefs.SetInt(JumpRestriction, 1);
                break;
            case Abilities.Dash:
                PlayerPrefs.SetInt(DashRestriction, 1);
                break;
            case Abilities.Wallrun:
                PlayerPrefs.SetInt(WallRunRestriction, 1);
                break;
            case Abilities.Crouch:
                PlayerPrefs.SetInt(CrouchRestriction, 1);
                break;
            case Abilities.Slide:
                PlayerPrefs.SetInt(SlideRestriction, 1);
                break;
            case Abilities.Fire:
                PlayerPrefs.SetInt(FireRestriction, 1);
                break;
            case Abilities.Reload:
                PlayerPrefs.SetInt(ReloadRestriction, 1);
                break;
        }
    }


#if UNITY_EDITOR
    [ContextMenu("Take Away All Abilities")]
    private void TakeWayAllAbilities()
    {
        IList enumList = Enum.GetValues(typeof(Abilities));
        foreach (Abilities ab in enumList)
        {
            TakeAbility(ab);
        }
    }

    [ContextMenu("Give All Abilities")]
    private void GiveAllAbilities()
    {
        IList enumList = Enum.GetValues(typeof(Abilities));
        foreach (Abilities ab in enumList)
        {
            GiveAbility(ab);
        }
    }
#endif

    private void TakeAbility(Abilities nextUnlock)
    {
        switch (nextUnlock)
        {
            case Abilities.ForwardWalk:
                PlayerPrefs.SetInt(ForwardRestriction, 0);
                break;
            case Abilities.HorizontalLook:
                PlayerPrefs.SetInt(LookHorizontalRestriction, 0);
                break;
            case Abilities.VerticalLook:
                PlayerPrefs.SetInt(LookVerticalRestriction, 0);
                break;
            case Abilities.Strafe:
                PlayerPrefs.SetInt(StrafeRestriction, 0);
                break;
            case Abilities.Jump:
                PlayerPrefs.SetInt(JumpRestriction, 0);
                break;
            case Abilities.Dash:
                PlayerPrefs.SetInt(DashRestriction, 0);
                break;
            case Abilities.Wallrun:
                PlayerPrefs.SetInt(WallRunRestriction, 0);
                break;
            case Abilities.Crouch:
                PlayerPrefs.SetInt(CrouchRestriction, 0);
                break;
            case Abilities.Slide:
                PlayerPrefs.SetInt(SlideRestriction, 0);
                break;
            case Abilities.Fire:
                PlayerPrefs.SetInt(FireRestriction, 0);
                break;
            case Abilities.Reload:
                PlayerPrefs.SetInt(ReloadRestriction, 0);
                break;
        }
    }
}
