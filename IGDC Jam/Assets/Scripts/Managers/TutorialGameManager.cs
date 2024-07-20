using Dialogue;
using UnityEngine;

public class TutorialGameManager : GameManager
{
    [SerializeField]
    private string _introKey;


    protected override void Start()
    {
        base.Start();
        DialogueManager.instance.OnDialogueComplete += OnIntroCompleted;
        DialogueManager.instance.StartDialogue(_introKey);
        TakeWayAllAbilities();
    }

    private void OnIntroCompleted(string key)
    {
        Debug.Log("Intro Completed!");
        GiveAbility(Abilities.ForwardWalk);
        SetAllRestrictions();
    }
}
