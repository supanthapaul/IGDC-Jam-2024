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
        
        private Coroutine _currentConversation;

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

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3f);
            StartDialogue("test");
            yield return new WaitForSeconds(3f);
            StartDialogue("test");
        }

        public void StartDialogue(string sceneContext)
        {
            var startNode = _dialogues.Find(d => d.key.Equals(sceneContext));

            if (startNode == null) return;
            if (_currentConversation != null)
            {
                StopCoroutine(_currentConversation);
                _audioSource.Stop();
            }
            _currentConversation = StartCoroutine(StartConversation(startNode));
        }

        private IEnumerator StartConversation(DialogueSO startNode)
        {
            _audioSource.Play();
            _audioSource.clip = startNode.audio;
            foreach(var dialogue in startNode.dialogues)
            {
                _audioText.SetText(dialogue.text);
                yield return new WaitForSecondsRealtime(dialogue.duration);
            }
        }
    }
}