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
        if (!key.Equals(_introKey)) return;
        GiveAbility(Abilities.ForwardWalk);
        SetAllRestrictions();
    }
}
