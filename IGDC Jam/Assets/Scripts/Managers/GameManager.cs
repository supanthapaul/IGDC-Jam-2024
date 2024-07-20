using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI abilityText;
    public CanvasGroup abilityAlphaGroup;



    private List<Abilities> abilitiesGotten;
    protected virtual void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void GetAbilityFeedBack(Abilities newAbility)
    {
        string unlockString = newAbility.ToString() + " unlocked";
        abilityText.text = unlockString;
        StartCoroutine(ShowAbilityNotif());
    }

    protected IEnumerator ShowAbilityNotif()
    {
        abilityAlphaGroup.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while(elapsedTime < 2f)
        {
            abilityAlphaGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / 2f);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        abilityAlphaGroup.gameObject.SetActive(false);

    }
}
