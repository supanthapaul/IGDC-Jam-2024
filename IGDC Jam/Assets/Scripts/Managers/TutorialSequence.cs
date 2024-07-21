using Dialogue;
using UnityEngine;

namespace Managers
{
    public class TutorialSequence : MonoBehaviour
    {
        [SerializeField]
        private string _introKey;

        [SerializeField]
        private string _successKey;

        private void Start()
        {
            DialogueManager.Instance.OnDialogueComplete += OnIntroCompleted;
            DialogueManager.Instance.StartDialogue(_introKey);
        }

        private void OnIntroCompleted(string key)
        {
            if (key.Equals(_introKey))
            {
                GameManager.Instance.GiveAbility(Abilities.ForwardWalk);
                GameManager.Instance.SetAllRestrictions();
                DialogueManager.Instance.StartDialogue("next_key");
            }
        }
    }
}
