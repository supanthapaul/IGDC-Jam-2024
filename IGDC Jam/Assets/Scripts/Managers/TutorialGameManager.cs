using Dialogue;
using UnityEngine;

public class TutorialGameManager : GameManager
{
    [SerializeField]
    private Ability _ability;

    [SerializeField]
    private string _introKey;


    protected override void Start()
    {
        base.Start();
        DialogueManager.instance.OnDialogueComplete += OnIntroCompleted;
        DialogueManager.instance.StartDialogue(_introKey);
    }

    private void OnIntroCompleted(string key)
    {
        if (!_introKey.Equals(key))
        {
            return;
        }
        Debug.Log("Intro Completed!");
        GiveAbility(Abilities.ForwardWalk);
        SetAllRestrictions();
        DialogueManager.instance.OnDialogueComplete -= OnIntroCompleted;
    }
}
