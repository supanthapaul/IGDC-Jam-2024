
using UnityEngine;

namespace LockAndDoor
{
    
    public abstract class IOpener: MonoBehaviour
    {
        public abstract void SetOpen(bool isOpen);
    }
}