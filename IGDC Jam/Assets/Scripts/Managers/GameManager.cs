using DG.Tweening;
using Health_System;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerPrefStatics;

public class GameManager : MonoBehaviour
{
    public FPController playerController;
    private IHealth playerHealth;
    public static GameManager Instance;
    public TextMeshProUGUI abilityText;
    public CanvasGroup abilityAlphaGroup;


    private AbilityUpdate[] abilities;

    private List<Abilities> abilitiesGottenThisRetry;
    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    protected virtual void Start()
    {
        abilitiesGottenThisRetry = new List<Abilities>();
        playerHealth = playerController.GetComponent<IHealth>();
        abilities = FindObjectsByType<AbilityUpdate>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    public void LevelCompleted()
    {
        abilitiesGottenThisRetry.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void PlayerDeath()
    {
        //some other stuff
        ResetTemporaryAbilities();

        ReloadStage();
    }

    private void ReloadStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetTemporaryAbilities()
    {
        foreach (Abilities ability in abilitiesGottenThisRetry)
        {
            RemoveAbility(ability);
        }
        abilitiesGottenThisRetry.Clear();
    }

    public void RemoveAbility(Abilities nextUnlock)
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

    private void GetAbilityFeedBack(Abilities newAbility)
    {
        string unlockString = newAbility.ToString() + " unlocked";
        abilityText.text = unlockString;
        abilitiesGottenThisRetry.Add(newAbility);
        StartCoroutine(ShowAbilityNotif());
    }

    protected IEnumerator ShowAbilityNotif()
    {
        abilityAlphaGroup.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            abilityAlphaGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / 2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        abilityAlphaGroup.gameObject.SetActive(false);

    }

    public void GiveAbility(Abilities nextUnlock)
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

        if(Application.isPlaying)
            GetAbilityFeedBack(nextUnlock);
    }

    public void SetAllRestrictions()
    {
        if (!Application.isPlaying) return;
        
        foreach (var ability in abilities)
        {
            ability.SetUpRestrictions();
        }
    }

    [ContextMenu("Take Away All Abilities")]
    protected void TakeWayAllAbilities()
    {
        IList enumList = Enum.GetValues(typeof(Abilities));
        foreach (Abilities ab in enumList)
        {
            RemoveAbility(ab);
        }
        SetAllRestrictions();
    }

    [ContextMenu("Give All Abilities")]
    protected void GiveAllAbilities()
    {
        IList enumList = Enum.GetValues(typeof(Abilities));
        foreach (Abilities ab in enumList)
        {
            GiveAbility(ab);
        }
        SetAllRestrictions();
    }

}
