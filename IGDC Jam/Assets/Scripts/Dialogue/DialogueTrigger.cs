using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        private string key;
        [SerializeField]
        private BoxCollider boxCollider;

        private void Start()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            DialogueManager.instance.StartDialogue(key);
            boxCollider.enabled = false;
        }

        private void OnReset()
        {
            boxCollider.enabled = true;
        }
    }
}
