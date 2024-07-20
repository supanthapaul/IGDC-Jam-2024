using System;
using Dialogue;
using UnityEngine;

public class TutorialGameManager : GameManager
{
    [SerializeField]
    private Ability _ability;

    [SerializeField]
    private string _introKey;
    

    private void Start()
    {
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
        _ability.GiveAbility(Abilities.ForwardWalk);
        DialogueManager.instance.OnDialogueComplete -= OnIntroCompleted;
    }
}
