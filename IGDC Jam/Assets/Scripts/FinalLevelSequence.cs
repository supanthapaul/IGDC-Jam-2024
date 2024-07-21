using Dialogue;
using Enemy;
using LockAndDoor;
using UnityEngine;

public class FinalLevelSequence : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;

    [SerializeField]
    private Door exitDoor;
    
    private GameManager _gameManager;
    private DialogueManager _dialogueManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _dialogueManager = DialogueManager.Instance;
        _dialogueManager.OnDialogueComplete += OnDialogueComplete;
    }

    private void OnDialogueComplete(string obj)
    {
        
    }
}
