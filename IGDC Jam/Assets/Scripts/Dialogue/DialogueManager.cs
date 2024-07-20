using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField]
        private List<DialogueSO> _dialogues;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private TextMeshProUGUI _audioText;
        
        public static DialogueManager instance;
        
        public Action<string> OnDialogueComplete;
        
        private Coroutine _currentConversation;
        private string _currentDialogueKey;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartDialogue(string dialogueKey)
        {
            var startNode = _dialogues.Find(d => d.key.Equals(dialogueKey));

            if (startNode == null) return;
            if (_currentConversation != null)
            {
                StopCoroutine(_currentConversation);
                _audioSource.Stop();
                OnDialogueComplete?.Invoke(_currentDialogueKey);
            }

            _currentDialogueKey = dialogueKey;
            _currentConversation = StartCoroutine(StartConversation(startNode));
        }

        private IEnumerator StartConversation(DialogueSO startNode)
        {
            _audioSource.clip = startNode.audio;
            _audioSource.Play();
            foreach(var dialogue in startNode.dialogues)
            {
                _audioText.SetText(dialogue.text);
                yield return new WaitForSecondsRealtime(dialogue.duration);
            }
            OnDialogueComplete?.Invoke(_currentDialogueKey);
        }
    }
}