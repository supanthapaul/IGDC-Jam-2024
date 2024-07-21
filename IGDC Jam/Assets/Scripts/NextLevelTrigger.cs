using Health_System;
using LockAndDoor;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField]
    private Door _door;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !other.TryGetComponent(out IHealth _)) return;
        if(_door.IsOpen)
            GameManager.Instance.LevelCompleted();
    }
}
