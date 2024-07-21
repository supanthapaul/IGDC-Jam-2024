using Health_System;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&other.TryGetComponent(out IHealth _))
        {
            GameManager.Instance.LevelCompleted();
        }
    }
}
