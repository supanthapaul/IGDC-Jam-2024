using UnityEngine;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        private string key;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            DialogueManager.instance.StartDialogue(key);
        }
    }
}
