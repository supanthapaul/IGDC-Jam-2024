
using Audio;
using UnityEngine;

namespace LockAndDoor
{
    
    public abstract class IOpener: MonoBehaviour
    {
        [SerializeField]
        private string openSoundName = "doorOpen";
        [SerializeField]
        private string closeSoundName = "doorClose";

        public virtual void SetOpen(bool isOpen)
        {
            if(isOpen && !string.IsNullOrEmpty(openSoundName))
                AudioManager.instance.PlaySound(openSoundName, transform.position);
            if(!isOpen && !string.IsNullOrEmpty(closeSoundName))
                AudioManager.instance.PlaySound(closeSoundName, transform.position);
        }
    }
}