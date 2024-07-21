using DG.Tweening;
using Health_System;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerPrefStatics;

public class GameManager : MonoBehaviour
{
    public FPController playerController;
    private IHealth playerHealth;
    public static GameManager Instance;
    public TextMeshProUGUI abilityText;
    [SerializeField]
    private Image healthBarFill;
    public CanvasGroup abilityAlphaGroup;
    public Image deathFade;

    public InputImages inputImages;
    
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
        deathFade.DOColor(Color.clear, 1f);
        SetUpInputImages();
        abilityAlphaGroup.alpha = 0f;
        abilityAlphaGroup.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerHealth != null) healthBarFill.fillAmount = playerHealth.currentHealth / playerHealth.totalHealth;
    }

    private void SetUpInputImages()
    {
        inputImages.ForwardWalk.gameObject.SetActive(PlayerPrefs.GetInt(ForwardRestriction, 0)==1);
        inputImages.Strafe.gameObject.SetActive(PlayerPrefs.GetInt(StrafeRestriction, 0)==1);
        inputImages.HorizontalLook.gameObject.SetActive(PlayerPrefs.GetInt(LookHorizontalRestriction, 0)==1);
        inputImages.VerticalLook.gameObject.SetActive(PlayerPrefs.GetInt(LookVerticalRestriction, 0)==1);
        inputImages.Reload.gameObject.SetActive(PlayerPrefs.GetInt(ReloadRestriction, 0)==1);
        inputImages.Equip.gameObject.SetActive(PlayerPrefs.GetInt(FireRestriction, 0)==1);
        inputImages.Dash.gameObject.SetActive(PlayerPrefs.GetInt(DashRestriction, 0)==1);
        inputImages.Slide.gameObject.SetActive(PlayerPrefs.GetInt(SlideRestriction, 0)==1);
        inputImages.Jump.gameObject.SetActive(PlayerPrefs.GetInt(JumpRestriction, 0)==1);
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
        deathFade.DOColor(new Color(0, 0, 0, 1), 1f).OnComplete(() => ReloadStage());
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
        string unlockString = newAbility.ToString();
        abilityText.text = unlockString;
        abilitiesGottenThisRetry.Add(newAbility);
        StartCoroutine(ShowAbilityNotif());
    }

    private IEnumerator ShowAbilityNotif()
    {
        abilityAlphaGroup.gameObject.SetActive(true);
        abilityAlphaGroup.transform.DOScaleX(1f, 0.35f).SetEase(Ease.OutBack);
        abilityAlphaGroup.DOFade(1f, 0.35f);

        yield return new WaitForSeconds(2f);
        abilityAlphaGroup.transform.DOScaleX(0f, 0.35f).SetEase(Ease.OutBack);
        abilityAlphaGroup.DOFade(0f, 0.35f).OnComplete(() =>
        {
            abilityAlphaGroup.gameObject.SetActive(false);
        });
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

        SetUpInputImages();

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
    protected void TakeAwayAllAbilities()
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

[Serializable]
public class InputImages
{
    public RectTransform ForwardWalk;
    public RectTransform HorizontalLook;
    public RectTransform Jump;
    public RectTransform Strafe;
    public RectTransform VerticalLook;
    public RectTransform Dash;
    public RectTransform Slide;
    public RectTransform Equip;
    public RectTransform Reload;
}
