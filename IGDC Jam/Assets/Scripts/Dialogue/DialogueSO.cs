using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogues/Dialogue", order = 0)]
    public class DialogueSO : ScriptableObject
    {
        [TextArea]
        public string text;
        public AudioClip audio;
        public float duration;
        public DialogueSO _nextDialogue;
        public string key;
    }
}