using Dialogue;
using UnityEngine;

public class TutorialGameManager : GameManager
{
    [SerializeField]
    private string _introKey;


    protected override void Start()
    {
        base.Start();
        TakeAwayAllAbilities();
        SetAllRestrictions();
        DialogueManager.Instance.OnDialogueComplete += OnIntroCompleted;
        DialogueManager.Instance.StartDialogue(_introKey);
        TakeAwayAllAbilities();
    }

    private void OnIntroCompleted(string key)
    {
        Debug.Log("Intro Completed!");
        GiveAbility(Abilities.ForwardWalk);
        SetAllRestrictions();
    }
}
