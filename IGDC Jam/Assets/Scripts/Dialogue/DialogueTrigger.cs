using Health_System;
using UnityEngine;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        private string key;
        [SerializeField]
        private BoxCollider boxCollider;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !other.TryGetComponent(out IHealth _ )) return;
            DialogueManager.Instance.StartDialogue(key);
            boxCollider.enabled = false;
        }

        private void OnReset()
        {
            boxCollider.enabled = true;
        }
    }
}
