using Dialogue;
using Enemy;
using LockAndDoor;
using UnityEngine;
using UnityEngine.Serialization;

public class FinalLevelSequence : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;

    [SerializeField]
    private Door exitDoor;

    [SerializeField]
    private int enemyCount;

    [SerializeField]
    private string onAllEnemyDeathDialogueKey;
    
    private GameManager _gameManager;
    private DialogueManager _dialogueManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _dialogueManager = DialogueManager.Instance;
        enemyManager.SpawnEnemies(enemyCount);
        enemyManager.OnAllEnemiesDeathEvent += OnAllEnemiesDeath;
    }

    private void OnAllEnemiesDeath()
    {
        _dialogueManager.StartDialogue(onAllEnemyDeathDialogueKey);
    }
}
